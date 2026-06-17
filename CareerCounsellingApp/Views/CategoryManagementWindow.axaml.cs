using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp;

public partial class CategoryManagementWindow : Window
{
    public CategoryManagementWindow()
    {
        InitializeComponent();
        DataContext = new CategoryManagementViewModel();
    }
}