using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using System;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Helpers;

public static class DialogHelper
{
    /// <summary>
    /// Shows a confirmation dialog
    /// </summary>
    public static async Task<bool> ShowConfirmationAsync(Window owner, string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            Background = Avalonia.Media.Brushes.White,
        };

        var mainPanel = new Border
        {
            Padding = new Thickness(30),
            Child = new StackPanel
            {
                Spacing = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Children =
                {
                    new TextBlock
                    {
                        Text = message,
                        FontSize = 14,
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                        Foreground = new Avalonia.Media.SolidColorBrush(new Avalonia.Media.Color(255, 17, 24, 39)),
                    }
                }
            }
        };

        bool result = false;

        var yesButton = new Button
        {
            Width = 80,
            Height = 40,
            Content = new TextBlock
            {
                Text = "Yes",
                Foreground = Avalonia.Media.Brushes.White,
                FontWeight = Avalonia.Media.FontWeight.SemiBold
            }
        };
        yesButton.Classes.Add("primary");

        var noButton = new Button
        {
            Width = 80,
            Height = 40,
            Content = new TextBlock
            {
                Text = "Cancel",
                Foreground = Avalonia.Media.Brushes.White,
                FontWeight = Avalonia.Media.FontWeight.SemiBold
            }
        };
        noButton.Classes.Add("error");

        yesButton.Click += (_, _) =>
        {
            result = true;
            dialog.Close();
        };

        noButton.Click += (_, _) =>
        {
            result = false;
            dialog.Close();
        };

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Right,
            Children = { yesButton, noButton }
        };

        var stackPanel = (StackPanel)mainPanel.Child;
        stackPanel.Children.Add(buttonPanel);

        dialog.Content = mainPanel;

        await dialog.ShowDialog(owner);
        return result;
    }

    /// <summary>
    /// Shows an information dialog
    /// </summary>
    public static async Task ShowInfoAsync(Window owner, string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            Background = Avalonia.Media.Brushes.White,
        };

        var mainPanel = new Border
        {
            Padding = new Thickness(30),
            Child = new StackPanel
            {
                Spacing = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Children =
                {
                    new TextBlock
                    {
                        Text = message,
                        FontSize = 14,
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                        Foreground = new Avalonia.Media.SolidColorBrush(new Avalonia.Media.Color(255, 17, 24, 39)),
                    }
                }
            }
        };

        var okButton = new Button
        {
            Width = 80,
            Height = 40,
            HorizontalAlignment = HorizontalAlignment.Right,
            Content = new TextBlock
            {
                Text = "OK",
                Foreground = Avalonia.Media.Brushes.White,
                FontWeight = Avalonia.Media.FontWeight.SemiBold
            }
        };
        okButton.Classes.Add("primary");

        okButton.Click += (_, _) =>
        {
            dialog.Close();
        };

        var stackPanel = (StackPanel)mainPanel.Child;
        stackPanel.Children.Add(okButton);

        dialog.Content = mainPanel;

        await dialog.ShowDialog(owner);
    }

    /// <summary>
    /// Shows an error dialog
    /// </summary>
    public static async Task ShowErrorAsync(Window owner, string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            Background = Avalonia.Media.Brushes.White,
        };

        var mainPanel = new Border
        {
            Padding = new Thickness(30),
            Child = new StackPanel
            {
                Spacing = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Children =
                {
                    new TextBlock
                    {
                        Text = message,
                        FontSize = 14,
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                        Foreground = new Avalonia.Media.SolidColorBrush(new Avalonia.Media.Color(255, 239, 68, 68)),
                    }
                }
            }
        };

        var okButton = new Button
        {
            Width = 80,
            Height = 40,
            HorizontalAlignment = HorizontalAlignment.Right,
            Content = new TextBlock
            {
                Text = "OK",
                Foreground = Avalonia.Media.Brushes.White,
                FontWeight = Avalonia.Media.FontWeight.SemiBold
            }
        };
        okButton.Classes.Add("error");

        okButton.Click += (_, _) =>
        {
            dialog.Close();
        };

        var stackPanel = (StackPanel)mainPanel.Child;
        stackPanel.Children.Add(okButton);

        dialog.Content = mainPanel;

        await dialog.ShowDialog(owner);
    }
}

