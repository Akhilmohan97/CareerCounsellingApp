using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp;

public partial class QuestionManagementWindow : Window
{
    private readonly QuestionManagementViewModel _viewModel;
    public QuestionManagementWindow()
    {
        InitializeComponent();
        _viewModel = new QuestionManagementViewModel();

        DataContext = _viewModel;

        var btn = this.FindControl<Button>("ManageOptionsButton");

        btn.Click += ManageOptionsClicked;
    }
    private void ManageOptionsClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_viewModel.SelectedQuestion == null)
            return;

        var window = new QuestionOptionManagementWindow(
            _viewModel.SelectedQuestion);

        window.Show();
    }
}