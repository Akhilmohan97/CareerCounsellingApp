using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp;

public partial class AssessmentResultWindow : Window
{
    public AssessmentResultWindow(int assessmentId)
    {
        InitializeComponent();

        DataContext = new AssessmentResultViewModel(assessmentId);
    }
}