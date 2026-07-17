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

public class QuestionManagementViewModel : INotifyPropertyChanged
{
    private string _questionText = "";
    private Category? _selectedCategory;
    private string _deleteMessage = "";

    private Question? _selectedQuestion;

    public string DeleteMessage
    {
        get => _deleteMessage;
        set
        {
            _deleteMessage = value;
            OnPropertyChanged(nameof(DeleteMessage));
        }
    }

    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set
        {
            _selectedQuestion = value;
            if(value != null)
            {
                QuestionText = value.QuestionText;
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == value.CategoryId);
               QuestionTextMalayalam = value.QuestionTextMalayalam;
                OnPropertyChanged(nameof(QuestionText));
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(QuestionTextMalayalam));
            }
            OnPropertyChanged(nameof(SelectedQuestion));
        }
    }
    public string QuestionText
    {
        get => _questionText;
        set
        {
            _questionText = value;
            OnPropertyChanged(nameof(QuestionText));
        }
    }
    private string _questionTextMalayalam;

    public string QuestionTextMalayalam
    {
        get { return _questionTextMalayalam; }
        set 
        {
            _questionTextMalayalam = value;
            OnPropertyChanged(nameof(QuestionTextMalayalam));
        }
    }

    public Category? SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            _selectedCategory = value;
            OnPropertyChanged(nameof(SelectedCategory));
        }
    }

    public ObservableCollection<Category> Categories { get; set; }
        = new();

    public ObservableCollection<Question> Questions { get; set; }
        = new();

    public ICommand AddQuestionCommand { get; }
    public ICommand DeleteQuestionCommand { get; }

    public QuestionManagementViewModel()
    {
        AddQuestionCommand = new RelayCommand(AddQuestion);
        DeleteQuestionCommand = new RelayCommand(DeleteQuestion);

        LoadCategories();
        LoadQuestions();
    }

    private void LoadCategories()
    {
        using var db = new AppDbContext();

        Categories.Clear();

        foreach (var category in db.Categories)
        {
            Categories.Add(category);
        }
    }

    private void LoadQuestions()
    {
        using var db = new AppDbContext();

        Questions.Clear();

        foreach (var question in db.Questions)
        {
            Questions.Add(question);
        }
    }

    private void AddQuestion()
    {
        if (string.IsNullOrWhiteSpace(QuestionText))
            return;

        if (SelectedCategory == null)
            return;

        using var db = new AppDbContext();

        db.Questions.Add(new Question
        {
            QuestionText = QuestionText,
            CategoryId = SelectedCategory.Id,
            QuestionTextMalayalam = QuestionTextMalayalam

        });

        db.SaveChanges();

        QuestionText = "";

        LoadQuestions();

        OnPropertyChanged(nameof(QuestionText));
    }

    private void DeleteQuestion()
    {
        if (SelectedQuestion == null)
            return;

        using var db = new AppDbContext();

        // Check if question has been answered by any student
        var hasAnswers = db.StudentAnswers
            .Any(sa => sa.QuestionId == SelectedQuestion.Id);

        if (hasAnswers)
        {
            // Show error message - question cannot be deleted
            DeleteMessage = $"❌ Cannot delete this question because {db.StudentAnswers.Count(sa => sa.QuestionId == SelectedQuestion.Id)} student(s) have already answered it.";
            return;
        }

        var questionToDelete = db.Questions
            .FirstOrDefault(q => q.Id == SelectedQuestion.Id);

        if (questionToDelete != null)
        {
            db.Questions.Remove(questionToDelete);
            db.SaveChanges();
            DeleteMessage = "✓ Question deleted successfully!";
        }

        QuestionText = String.Empty;
        SelectedCategory = null;
        SelectedQuestion = null;
        QuestionTextMalayalam = String.Empty;

        LoadQuestions();

        OnPropertyChanged(nameof(QuestionText));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
