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

public class QuestionOptionManagementViewModel : INotifyPropertyChanged
{

    private QuestionOption? _selectedOption;

    public QuestionOption? SelectedOption
    {
        get => _selectedOption;
        set
        {
            _selectedOption = value;

            if (value != null)
            {
                OptionText = value.OptionText;
                Score = value.Score;

                OnPropertyChanged(nameof(OptionText));
                OnPropertyChanged(nameof(Score));
            }

            OnPropertyChanged(nameof(SelectedOption));
        }
    }

    private readonly Question _question;

    private string _optionText = "";
    private int _score;

    public string OptionText
    {
        get => _optionText;
        set
        {
            _optionText = value;
            OnPropertyChanged(nameof(OptionText));
        }
    }

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnPropertyChanged(nameof(Score));
        }
    }

    public ObservableCollection<QuestionOption> Options { get; }
        = new();

    public ICommand AddOptionCommand { get; }
    public ICommand UpdateOptionCommand { get; }
    public ICommand DeleteOptionCommand { get; }

    public QuestionOptionManagementViewModel(Question question)
    {
        _question = question;

        AddOptionCommand = new RelayCommand(AddOption);
        UpdateOptionCommand = new RelayCommand(UpdateOption);
        DeleteOptionCommand = new RelayCommand(DeleteOption);

        LoadOptions();
    }
    private void UpdateOption()
    {
        if (SelectedOption == null)
            return;

        using var db = new AppDbContext();

        var option = db.QuestionOptions
            .FirstOrDefault(x => x.Id == SelectedOption.Id);

        if (option == null)
            return;

        option.OptionText = OptionText;
        option.Score = Score;

        db.SaveChanges();

        LoadOptions();
    }
    private void DeleteOption()
    {
        if (SelectedOption == null)
            return;

        using var db = new AppDbContext();

        var option = db.QuestionOptions
            .FirstOrDefault(x => x.Id == SelectedOption.Id);

        if (option == null)
            return;

        db.QuestionOptions.Remove(option);

        db.SaveChanges();

        OptionText = "";
        Score = 0;
        SelectedOption = null;

        LoadOptions();
    }
    private void LoadOptions()
    {
        using var db = new AppDbContext();

        Options.Clear();

        foreach (var option in db.QuestionOptions
                     .Where(x => x.QuestionId == _question.Id))
        {
            Options.Add(option);
        }
    }

    private void AddOption()
    {
        if (string.IsNullOrWhiteSpace(OptionText))
            return;

        using var db = new AppDbContext();

        db.QuestionOptions.Add(new QuestionOption
        {
            QuestionId = _question.Id,
            OptionText = OptionText,
            Score = Score
        });

        db.SaveChanges();

        OptionText = "";
        Score = 0;

        LoadOptions();

        OnPropertyChanged(nameof(OptionText));
        OnPropertyChanged(nameof(Score));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
