using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.ViewModels;
using Velopack;

namespace CareerCounsellingApp;

public partial class UpdateWindow : Window
{
    public UpdateWindow(UpdateInfo update)
    {
        InitializeComponent();

        DataContext = new UpdateViewModel(update);
    }
}