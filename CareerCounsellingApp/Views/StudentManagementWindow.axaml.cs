using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.Models;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp;

public partial class StudentManagementWindow : Window
{
    public StudentManagementWindow()
    {
        InitializeComponent();
        DataContext = new StudentManagementViewModel();
    }
}