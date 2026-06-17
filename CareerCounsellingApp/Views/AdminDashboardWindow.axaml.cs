using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.Helpers;
using CareerCounsellingApp.Views;
using CareerCounsellingApp.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace CareerCounsellingApp;

public partial class AdminDashboardWindow : Window
{
    private AdminDashboardViewModel? _viewModel;

    public AdminDashboardWindow()
    {
        InitializeComponent();
        
        // Set DataContext to the ViewModel
        _viewModel = new AdminDashboardViewModel();
        DataContext = _viewModel;
        
        SetupEventHandlers();
    }

    private void SetupEventHandlers()
    {
        var btn = this.FindControl<Button>("CategoryButton");
        btn.Click += (_, _) =>
        {
            new CategoryManagementWindow().Show();
        };

        var questionBtn = this.FindControl<Button>("QuestionButton");
        questionBtn.Click += (_, _) =>
        {
            new QuestionManagementWindow().Show();
        };

        var studentButton = this.FindControl<Button>("StudentButton");
        studentButton.Click += (_, _) =>
        {
            new StudentManagementWindow().Show();
        };

        var assessmentResultsButton = this.FindControl<Button>("AssessmentResultsButton");
        assessmentResultsButton.Click += (_, _) =>
        {
            new AssessmentResultsWindow().Show();
        };

        // Add logout button handler
        var logoutButton = this.FindControl<Button>("LogoutButton");
        if (logoutButton != null)
        {
            logoutButton.Click += LogoutClicked;
        }

        // Add refresh button handler
        var refreshButton = this.FindControl<Button>("RefreshButton");
        if (refreshButton != null)
        {
            refreshButton.Click += RefreshClicked;
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

    private void RefreshClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _viewModel?.ReloadStatistics();
    }
}



