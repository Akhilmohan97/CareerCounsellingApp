using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.PhotoCapture.Interfaces;
using CareerCounsellingApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CareerCounsellingApp;

public partial class PhotoCaptureWindow : Window
{
    public PhotoCaptureWindow()
    {
        InitializeComponent();
        DataContext = new PhotoCaptureViewModel();
    }

   
}