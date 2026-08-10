using CareerCounsellingApp.Data;
using CareerCounsellingApp.Helpers;
using CareerCounsellingApp.Models;
using CareerCounsellingApp.Services.Assessment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace CareerCounsellingApp.ViewModels;

public class AssessmentViewModel : INotifyPropertyChanged
{
    private readonly Student _student;
    private readonly Action? _onAssessmentSubmitted;
    private bool _useMalayalam;
    private int _currentQuestionIndex = 0;

    public ObservableCollection<AssessmentQuestion>
        Questions
    { get; } = new();

    private AssessmentQuestion? _currentQuestion;
    public AssessmentQuestion? CurrentQuestion
    {
        get => _currentQuestion;
        set
        {
            if (_currentQuestion == value) return;
            _currentQuestion = value;
            OnPropertyChanged(nameof(CurrentQuestion));
        }
    }

    public bool UseMalayalam
    {
        get => _useMalayalam;
        set
        {
            if (_useMalayalam == value) return;
            _useMalayalam = value;
            OnPropertyChanged(nameof(UseMalayalam));
            UpdateAllQuestionsLanguage();
        }
    }

    public int AnsweredCount => Questions.Count(q => q.SelectedOption != null);
    
    public int TotalQuestions => Questions.Count;
    
    public string ProgressText => $"{AnsweredCount} of {TotalQuestions} answered";

    public int CurrentQuestionNumber => _currentQuestionIndex + 1;

    public bool CanGoNext => _currentQuestionIndex < TotalQuestions - 1;
    public bool CanGoPrevious => _currentQuestionIndex > 0;
    public bool CanSubmit => AnsweredCount == TotalQuestions && _currentQuestionIndex == TotalQuestions - 1;

    public ICommand SubmitAssessmentCommand { get; }
    public ICommand NextQuestionCommand { get; }
    public ICommand PreviousQuestionCommand { get; }

    public AssessmentViewModel(Student student, Action? onAssessmentSubmitted = null)
    {
        _student = student;
        _onAssessmentSubmitted = onAssessmentSubmitted;

        SubmitAssessmentCommand =
            new RelayCommand(SubmitAssessment, () => CanSubmit);
        NextQuestionCommand =
            new RelayCommand(GoToNextQuestion, () => CanGoNext);
        PreviousQuestionCommand =
            new RelayCommand(GoToPreviousQuestion, () => CanGoPrevious);

        LoadQuestions();
    }
    private void GoToNextQuestion()
    {
        if (CanGoNext)
        {
            _currentQuestionIndex++;
            CurrentQuestion = Questions[_currentQuestionIndex];
            UpdateNavigationCommands();
        }
    }

    private void GoToPreviousQuestion()
    {
        if (CanGoPrevious)
        {
            _currentQuestionIndex--;
            CurrentQuestion = Questions[_currentQuestionIndex];
            UpdateNavigationCommands();
        }
    }

    private void UpdateNavigationCommands()
    {
        OnPropertyChanged(nameof(CanGoNext));
        OnPropertyChanged(nameof(CanGoPrevious));
        OnPropertyChanged(nameof(CanSubmit));
        OnPropertyChanged(nameof(CurrentQuestionNumber));
        ((RelayCommand)NextQuestionCommand).RaiseCanExecuteChanged();
        ((RelayCommand)PreviousQuestionCommand).RaiseCanExecuteChanged();
        ((RelayCommand)SubmitAssessmentCommand).RaiseCanExecuteChanged();
    }
    private async Task LoadQuestions()
    {
        using var db = new AppDbContext();

        Questions.Clear();

        // Buffer the outer query and eager-load the options to avoid nested commands
        var questions = db.Questions
            .Include(q => q.Image)
            .Include(q => q.Options)
            .AsNoTracking()
            .ToList();

        foreach (var question in questions)
        {
            var assessmentQuestion =
                new AssessmentQuestion
                {
                    Question = question
                };

            // assign sequential number starting from 1
            assessmentQuestion.Number = Questions.Count + 1;

            assessmentQuestion.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(AssessmentQuestion.SelectedOption))
                {
                    OnPropertyChanged(nameof(AnsweredCount));
                    OnPropertyChanged(nameof(ProgressText));

                    UpdateNavigationCommands();

                    if (assessmentQuestion.SelectedOption != null &&
                        _currentQuestionIndex < Questions.Count - 1)
                    {
                        await Task.Delay(200);
                        GoToNextQuestion();
                    }
                }
            };

            foreach (var option in question.Options)
            {
                var assessmentOption = new AssessmentOption(option)
                {
                    UseMalayalam = _useMalayalam
                };
                assessmentQuestion.Options.Add(assessmentOption);
            }

            Questions.Add(assessmentQuestion);
        }

        // Set the first question as current
        if (Questions.Count > 0)
        {
            _currentQuestionIndex = 0;
            CurrentQuestion = Questions[0];
            UpdateNavigationCommands();
        }
    }

    private void UpdateAllQuestionsLanguage()
    {
        foreach (var question in Questions)
        {
            question.UseMalayalam = _useMalayalam;
        }
    }

    private async void SubmitAssessment()
    {
        using var db = new AppDbContext();

        var assessment = new Assessment
        {
            StudentId = _student.Id,
            AssessmentDate = DateTime.UtcNow
        };

        db.Assessments.Add(assessment);
        db.SaveChanges();

        foreach (var question in Questions)
        {
            if (question.SelectedOption == null)
                continue;

            db.StudentAnswers.Add(
                new StudentAnswer
                {
                    AssessmentId = assessment.Id,
                    QuestionId = question.Question.Id,
                    QuestionOptionId =
                        question.SelectedOption.Id
                });
        }

        await db.SaveChangesAsync();
        var assessmentEngine = new AssessmentEngine(
                                db,
                                new ScoreCalculator());

        await assessmentEngine.CalculateAsync(assessment.Id);
        var thankYou = new ThankYouWindow(_onAssessmentSubmitted);
        thankYou.Show();
    }

    public event PropertyChangedEventHandler?
        PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
