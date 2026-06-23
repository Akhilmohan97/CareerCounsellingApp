using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CareerCounsellingApp;

public partial class ParentCategoryManagementWindow : Window
{
    public ParentCategoryManagementWindow()
    {
        InitializeComponent();
        DataContext = new ViewModels.ParentCategoryManagementViewModel();
    }
}