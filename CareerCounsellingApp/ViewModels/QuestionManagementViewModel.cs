using CareerCounsellingApp.Data;
using CareerCounsellingApp.Helpers;
using CareerCounsellingApp.Models;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace CareerCounsellingApp.ViewModels;

public class QuestionManagementViewModel : INotifyPropertyChanged
{
    private string _questionText = "";
    private Category? _selectedCategory;
    private string _deleteMessage = "";
    private byte[]? _questionImageData;
    private Bitmap? _questionImagePreview;

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
                QuestionImageData = value.Image?.ImageData;
                OnPropertyChanged(nameof(QuestionText));
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(QuestionTextMalayalam));
            }
            else
            {
                QuestionImageData = null;
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

    public byte[]? QuestionImageData
    {
        get => _questionImageData;
        set
        {
            _questionImageData = value;
            QuestionImagePreview = CreateBitmap(value);
            OnPropertyChanged(nameof(QuestionImageData));
            OnPropertyChanged(nameof(HasQuestionImage));
        }
    }

    public Bitmap? QuestionImagePreview
    {
        get => _questionImagePreview;
        private set
        {
            _questionImagePreview = value;
            OnPropertyChanged(nameof(QuestionImagePreview));
        }
    }

    public bool HasQuestionImage => QuestionImageData != null && QuestionImageData.Length > 0;

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
    public ICommand SelectImageCommand { get; }
    public ICommand RemoveImageCommand { get; }

    public QuestionManagementViewModel()
    {
        AddQuestionCommand = new RelayCommand(AddQuestion);
        DeleteQuestionCommand = new RelayCommand(DeleteQuestion);
        SelectImageCommand = new RelayCommand(SelectImage);
        RemoveImageCommand = new RelayCommand(RemoveImage);

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

        foreach (var question in db.Questions.Include(q => q.Image))
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

        var question = new Question
        {
            QuestionText = QuestionText,
            CategoryId = SelectedCategory.Id,
            QuestionTextMalayalam = QuestionTextMalayalam
        };

        db.Questions.Add(question);

        if (QuestionImageData != null && QuestionImageData.Length > 0)
        {
            question.Image = new QuestionImage
            {
                ImageData = QuestionImageData
            };
        }

        db.SaveChanges();

        QuestionText = "";
        QuestionTextMalayalam = string.Empty;
        QuestionImageData = null;
        SelectedQuestion = null;

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
        QuestionImageData = null;

        LoadQuestions();

        OnPropertyChanged(nameof(QuestionText));
    }

    private void RemoveImage()
    {
        QuestionImageData = null;
    }

    private async void SelectImage()
    {
        var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var owner = lifetime?.Windows.LastOrDefault(x => x.IsActive) ?? lifetime?.Windows.LastOrDefault();

        if (owner?.StorageProvider == null)
            return;

        var files = await owner.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Question Image",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Image Files")
                {
                    Patterns = ["*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif", "*.webp"]
                }
            ]
        });

        var file = files.FirstOrDefault();
        if (file == null)
            return;

        await using var stream = await file.OpenReadAsync();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        QuestionImageData = memoryStream.ToArray();
    }

    private static Bitmap? CreateBitmap(byte[]? imageData)
    {
        if (imageData == null || imageData.Length == 0)
            return null;

        return new Bitmap(new MemoryStream(imageData));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
