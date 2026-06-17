using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CareerCounsellingApp;

public partial class AlreadySubmittedWindow : Window
{
    public AlreadySubmittedWindow()
    {
        InitializeComponent();

        var closeButton =
            this.FindControl<Button>("CloseButton");

        closeButton.Click += (_, _) =>
        {
            Close();
        };
    }
}