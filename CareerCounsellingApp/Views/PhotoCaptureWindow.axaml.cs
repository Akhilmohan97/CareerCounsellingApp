using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp;

public partial class PhotoCaptureWindow : Window
{
    public PhotoCaptureWindow()
    {
        InitializeComponent();
        DataContext = new PhotoCaptureViewModel();
    }
}