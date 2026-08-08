using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace CareerCounsellingApp.Models;

public class AssessmentQuestion : INotifyPropertyChanged
{
    private QuestionOption? _selectedOption;
    private int _number;
    private bool _useMalayalam;

    public Question Question { get; set; } = null!;

    public int Number
    {
        get => _number;
        set
        {
            _number = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
        }
    }

    public ObservableCollection<AssessmentOption> Options
    { get; set; } = new();

    public QuestionOption? SelectedOption
    {
        get => _selectedOption;
        set
        {
            _selectedOption = value;
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(SelectedOption)));
        }
    }

    public bool UseMalayalam
    {
        get => _useMalayalam;
        set
        {
            if (_useMalayalam == value) return;
            _useMalayalam = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseMalayalam)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayQuestionText)));
            foreach (var option in Options)
            {
                option.UseMalayalam = value;
            }
        }
    }

    public string DisplayQuestionText => UseMalayalam ? Question.QuestionTextMalayalam : Question.QuestionText;

    public Bitmap? QuestionImage => Question.ImageBitmap;

    public bool HasQuestionImage => Question.HasImage;

    public event PropertyChangedEventHandler? PropertyChanged;
}
