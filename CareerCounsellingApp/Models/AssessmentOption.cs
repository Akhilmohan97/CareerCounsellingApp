using System.ComponentModel;

namespace CareerCounsellingApp.Models;

public class AssessmentOption : INotifyPropertyChanged
{
    private bool _useMalayalam;

    public QuestionOption Option { get; }

    public AssessmentOption(QuestionOption option)
    {
        Option = option;
    }

    public bool UseMalayalam
    {
        get => _useMalayalam;
        set
        {
            if (_useMalayalam == value) return;
            _useMalayalam = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseMalayalam)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayText)));
        }
    }

    public string DisplayText => UseMalayalam ? Option.OptionTextMalayalam : Option.OptionText;

    public event PropertyChangedEventHandler? PropertyChanged;
}
