using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp;

public partial class AssessmentResultsWindow : Window
{
    public AssessmentResultsWindow()
    {
        InitializeComponent();

        DataContext =
            new AssessmentResultsViewModel();
    }
}