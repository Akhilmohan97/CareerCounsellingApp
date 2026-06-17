using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CareerCounsellingApp;

public partial class ThankYouWindow : Window
{
    public ThankYouWindow()
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