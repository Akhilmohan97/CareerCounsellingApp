using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Models;

public class AssessmentQuestion : INotifyPropertyChanged
{
    private QuestionOption? _selectedOption;

    public Question Question { get; set; } = null!;

    public ObservableCollection<QuestionOption> Options
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

    public event PropertyChangedEventHandler? PropertyChanged;
}
