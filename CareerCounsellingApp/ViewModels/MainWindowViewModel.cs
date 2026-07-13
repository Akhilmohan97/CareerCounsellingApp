using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CareerCounsellingApp.PhotoCapture.Services;
using CareerCounsellingApp.Services;
using CareerCounsellingApp.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace CareerCounsellingApp.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService = new();
    public MainWindow? LoginWindow { get; set; }

    private string _username = "";
    private string _password = "";
    private string _message = "";
    private bool _isLoading = false;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
            ((RelayCommand)LoginCommand).NotifyCanExecuteChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
            ((RelayCommand)LoginCommand).NotifyCanExecuteChanged();
        }
    }

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged(nameof(Message));
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
            ((RelayCommand)LoginCommand).NotifyCanExecuteChanged();
        }
    }

    public ICommand LoginCommand { get; }
    public ICommand ClearCommand { get; }

    public MainWindowViewModel()
    {
        LoginCommand = new RelayCommand(Login, CanLogin);
        ClearCommand = new RelayCommand(Clear);
    }

    private bool CanLogin()
    {
        return !IsLoading && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }

    private void Login()
    {
        var network = new NetworkService();

        var url = network.BuildCaptureUrl(Guid.NewGuid());

        Console.WriteLine(url);
        if (string.IsNullOrWhiteSpace(Username))
        {
            Message = "❌ Please enter a username";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            Message = "❌ Please enter a password";
            return;
        }

        IsLoading = true;
        try
        {
            var user = _authService.Login(Username, Password);

            if (user == null)
            {
                Message = "❌ Invalid username or password. Please try again.";
                Password = "";
                return;
            }

            Message = $"✓ Login successful! Redirecting...";

            if (user.Role == "Admin")
            {
                new AdminDashboardWindow().Show();
            }
            else if (user.Role == "Student")
            {
                new StudentDashboardWindow(user).Show();
            }

            // Close the login window after successful authentication
            LoginWindow?.Close();

            // Clear credentials after successful login
            Username = "";
            Password = "";
            Message = "";
        }
        catch (Exception ex)
        {
            Message = $"❌ Login error: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void Clear()
    {
        Username = "";
        Password = "";
        Message = "";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


