using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.Data;
using CareerCounsellingApp.Models;
using CareerCounsellingApp.ViewModels;
using System.Linq;

namespace CareerCounsellingApp;

public partial class StudentDashboardWindow : Window
{
    private readonly StudentDashboardViewModel _viewModel;

    public StudentDashboardWindow(User user)
    {
        InitializeComponent();

        _viewModel = new StudentDashboardViewModel(user);

        DataContext = _viewModel;

        var startButton =
            this.FindControl<Button>("StartAssessmentButton");

        startButton.Click += StartAssessmentClicked;
    }

    private void StartAssessmentClicked(
    object? sender,
    Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_viewModel.CurrentStudent == null)
            return;

        using var db = new AppDbContext();

        bool alreadySubmitted =
            db.Assessments.Any(x =>
                x.StudentId ==
                _viewModel.CurrentStudent.Id);

        if (alreadySubmitted)
        {
            new AlreadySubmittedWindow().Show();
            return;
        }

        var assessmentWindow =
            new AssessmentWindow(
                _viewModel.CurrentStudent);

        assessmentWindow.Show();
    }
}