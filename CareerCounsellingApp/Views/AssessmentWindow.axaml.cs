using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.Models;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp;


    public partial class AssessmentWindow : Window
    {
        public AssessmentWindow(Student student)
        {
            InitializeComponent();

            DataContext = new AssessmentViewModel(student, () => Close());
        }

        private void RadioButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Tag is QuestionOption selectedOption)
            {
                // Find the parent ItemsControl and get its DataContext (AssessmentQuestion)
                var parent = radioButton.Parent;
                while (parent != null && parent is not ItemsControl)
                {
                    parent = parent.Parent;
                }

                if (parent is ItemsControl itemsControl && itemsControl.DataContext is AssessmentQuestion assessmentQuestion)
                {
                    assessmentQuestion.SelectedOption = selectedOption;
                }
            }
        }
    }
