using CareerCounsellingApp.Services;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;

namespace CareerCounsellingApp.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService = new();

    private string _username = "";
    private string _password = "";
    private string _message = "";

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
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

    public ICommand LoginCommand { get; }

    public MainWindowViewModel()
    {
        LoginCommand = new RelayCommand(Login);
    }

    private void Login()
    {
        var user = _authService.Login(Username, Password);

        if (user == null)
        {
            Message = "Invalid username or password";
            return;
        }

        if (user.Role == "Admin")
        {
            new AdminDashboardWindow().Show();
        }
        else if (user.Role == "Student")
        {
            new StudentDashboardWindow(user).Show();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}