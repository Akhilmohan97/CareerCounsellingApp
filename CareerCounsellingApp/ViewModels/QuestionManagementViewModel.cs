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

    private Question? _selectedQuestion;

    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set
        {
            _selectedQuestion = value;
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

    public QuestionManagementViewModel()
    {
        AddQuestionCommand = new RelayCommand(AddQuestion);

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
            CategoryId = SelectedCategory.Id
        });

        db.SaveChanges();

        QuestionText = "";

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
