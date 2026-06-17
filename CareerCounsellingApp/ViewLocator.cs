using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CareerCounsellingApp.ViewModels;
using CareerCounsellingApp.Views;

namespace CareerCounsellingApp;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        return data switch
        {
            MainWindowViewModel => new MainWindow(),
            _ => null
        };
    }

    public bool Match(object? data)
    {
        return data is not null;
    }
}