using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.Models;
using CareerCounsellingApp.PhotoCapture.Interfaces;
using CareerCounsellingApp.PhotoCapture.Interfaces.Server;
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
            var _cordinator = AppServices.Provider.GetRequiredService<IPhotoCaptureCoordinator>();
            _cordinator.ShowCaptureWindowAsync(this);
    
        };
    }
}