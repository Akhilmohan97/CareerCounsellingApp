using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.ViewModels;
using System;

namespace CareerCounsellingApp;

public partial class CategoryManagementWindow : Window
{
    public CategoryManagementWindow()
    {
        InitializeComponent();
        DataContext = new CategoryManagementViewModel();
       // SetUpEventListeners();
    }

    private void SetUpEventListeners()
    {
        var btn = this.FindControl<Button>("AddParentCatBtn");
        btn.Click += (_, _) =>
        {
            new ParentCategoryManagementWindow().Show();
        };
    }
}