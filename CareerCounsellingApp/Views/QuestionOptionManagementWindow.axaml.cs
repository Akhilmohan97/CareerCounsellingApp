using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.Models;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp;

public partial class QuestionOptionManagementWindow : Window
{
    public QuestionOptionManagementWindow(Question question)
    {
        InitializeComponent();

        DataContext = new QuestionOptionManagementViewModel(question);
    }
}