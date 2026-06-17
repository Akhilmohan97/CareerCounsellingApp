using CareerCounsellingApp.Data;
using CareerCounsellingApp.Helpers;
using CareerCounsellingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CareerCounsellingApp.ViewModels;

public class AssessmentViewModel : INotifyPropertyChanged
{
    private readonly Student _student;
    private readonly Action? _onAssessmentSubmitted;

    public ObservableCollection<AssessmentQuestion>
        Questions
    { get; } = new();

    public int AnsweredCount => Questions.Count(q => q.SelectedOption != null);
    
    public int TotalQuestions => Questions.Count;
    
    public string ProgressText => $"{AnsweredCount} of {TotalQuestions} answered";

    public ICommand SubmitAssessmentCommand { get; }

    public AssessmentViewModel(Student student, Action? onAssessmentSubmitted = null)
    {
        _student = student;
        _onAssessmentSubmitted = onAssessmentSubmitted;

        SubmitAssessmentCommand =
            new RelayCommand(SubmitAssessment);

        LoadQuestions();
    }

    private void LoadQuestions()
    {
        using var db = new AppDbContext();

        Questions.Clear();

        foreach (var question in db.Questions)
        {
            var assessmentQuestion =
                new AssessmentQuestion
                {
                    Question = question
                };

            assessmentQuestion.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(AssessmentQuestion.SelectedOption))
                {
                    OnPropertyChanged(nameof(AnsweredCount));
                    OnPropertyChanged(nameof(ProgressText));
                }
            };

            foreach (var option in db.QuestionOptions
                         .Where(x => x.QuestionId == question.Id))
            {
                assessmentQuestion.Options.Add(option);
            }

            Questions.Add(assessmentQuestion);
        }
    }

    private void SubmitAssessment()
    {
        using var db = new AppDbContext();

        var assessment = new Assessment
        {
            StudentId = _student.Id,
            AssessmentDate = DateTime.Now
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

        db.SaveChanges();
        
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
