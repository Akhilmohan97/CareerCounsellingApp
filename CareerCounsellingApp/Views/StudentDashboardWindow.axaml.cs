using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.Data;
using CareerCounsellingApp.Helpers;
using CareerCounsellingApp.Models;
using CareerCounsellingApp.Views;
using CareerCounsellingApp.ViewModels;
using System.Collections.Generic;
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

        // Attach event handlers - fields are auto-generated from x:Name in XAML
        StartAssessmentButton.Click += StartAssessmentClicked;
        LogoutButton.Click += LogoutClicked;
    }

    private void StartAssessmentClicked(
    object? sender,
    Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            if (_viewModel?.CurrentStudent == null)
            {
                // Show error if student not found
                var errorWindow = new Window
                {
                    Title = "Error",
                    Width = 300,
                    Height = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Content = new TextBlock
                    {
                        Text = "Student information not found. Please log in again.",
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                        Margin = new Thickness(20)
                    }
                };
                errorWindow.ShowDialog(this);
                return;
            }

            using var db = new AppDbContext();

            // Check if questions exist
            var questionsExist = db.Questions.Any();
            if (!questionsExist)
            {
                var noQuestionsWindow = new Window
                {
                    Title = "No Questions Available",
                    Width = 350,
                    Height = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Content = new TextBlock
                    {
                        Text = "There are no assessment questions available yet. Please contact the administrator.",
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                        Margin = new Thickness(20)
                    }
                };
                noQuestionsWindow.ShowDialog(this);
                return;
            }

            bool alreadySubmitted =
                db.Assessments.Any(x =>
                    x.StudentId ==
                    _viewModel.CurrentStudent.Id);

            if (alreadySubmitted)
            {
                var alreadySubmittedWindow = new AlreadySubmittedWindow();
                alreadySubmittedWindow.Show();
                return;
            }

            var assessmentWindow =
                new AssessmentWindow(
                    _viewModel.CurrentStudent);

            assessmentWindow.Show();
        }
        catch (System.Exception ex)
        {
            // Show error dialog
            var errorWindow = new Window
            {
                Title = "Error",
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = new StackPanel
                {
                    Margin = new Thickness(20),
                    Children =
                    {
                        new TextBlock
                        {
                            Text = "An error occurred while starting the assessment:",
                            FontWeight = Avalonia.Media.FontWeight.Bold,
                            Margin = new Thickness(0, 0, 0, 10)
                        },
                        new TextBlock
                        {
                            Text = ex.Message,
                            TextWrapping = Avalonia.Media.TextWrapping.Wrap
                        }
                    }
                }
            };
            errorWindow.ShowDialog(this);
        }
    }

    private async void LogoutClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var confirmed = await DialogHelper.ShowConfirmationAsync(
            this,
            "Logout",
            "Are you sure you want to logout?"
        );

        if (confirmed)
        {
            // Get reference to all windows before closing this one
            List<Window> allWindows = new();
            if (Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                allWindows.AddRange(desktop.Windows);
            }

            // Close this window first
            Close();

            // Close all other windows
            foreach (var window in allWindows)
            {
                if (window != this)
                {
                    try
                    {
                        window.Close();
                    }
                    catch
                    {
                        // Window may already be closed
                    }
                }
            }

            // Create and show fresh login window
            var loginWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
            loginWindow.Show();
        }
    }
}


