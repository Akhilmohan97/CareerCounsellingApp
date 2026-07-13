using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.Models;
using CareerCounsellingApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CareerCounsellingApp;

public partial class StudentManagementWindow : Window
{
    public StudentManagementWindow()
    {
        InitializeComponent();
        DataContext = new StudentManagementViewModel();

        var btn = this.FindControl<Button>("BrowsePhotoButton");
        btn.Click += (_, _) =>
        {
            var window=AppServices.Provider.GetRequiredService<PhotoCaptureWindow>();
            window.ShowDialog(this);
        };
    }
}