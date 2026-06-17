using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace CareerCounsellingApp;

public partial class ThankYouWindow : Window
{
    private readonly Action? _onClose;

    public ThankYouWindow(Action? onClose = null)
    {
        InitializeComponent();
        
        _onClose = onClose;

        var closeButton =
            this.FindControl<Button>("CloseButton");

        closeButton.Click += (_, _) =>
        {
            _onClose?.Invoke();
            Close();
        };
    }
}