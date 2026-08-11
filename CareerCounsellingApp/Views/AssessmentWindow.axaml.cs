using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.Models;
using CareerCounsellingApp.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace CareerCounsellingApp;


    public partial class AssessmentWindow : Window
    {
        public AssessmentWindow(Student student)
        {
            InitializeComponent();

            var viewModel = new AssessmentViewModel(student, () => Close());
            DataContext = viewModel;
            
            // Subscribe to view model changes to restore radio button state
            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(AssessmentViewModel.CurrentQuestion))
                {
                    // Restore radio button selection for the current question
                    RestoreRadioButtonSelection();
                }
            };
        }

        private void RestoreRadioButtonSelection()
        {
            if (DataContext is AssessmentViewModel viewModel && viewModel.CurrentQuestion?.SelectedOption != null)
            {
                // Find and check the radio button that corresponds to the selected option
                var selectedOption = viewModel.CurrentQuestion.SelectedOption;
                
                // Schedule restoration on the next UI cycle to ensure controls are laid out
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    FindAndSelectRadioButton(selectedOption);
                });
            }
        }

        private void FindAndSelectRadioButton(QuestionOption targetOption)
        {
            // Find all RadioButtons in the visual tree and select the one matching the target option
            var dockPanel = this.FindControl<DockPanel>("DockPanelMain");
            if (dockPanel != null)
            {
                var radioButtons = FindAllDescendants<RadioButton>(dockPanel).ToList();
                foreach (var radioButton in radioButtons)
                {
                    if (radioButton.Tag is QuestionOption option && option.Id == targetOption.Id)
                    {
                        radioButton.IsChecked = true;
                        break;
                    }
                }
            }
        }

        private IEnumerable<T> FindAllDescendants<T>(ILogical visual) where T : class
        {
            foreach (var child in visual.LogicalChildren)
            {
                if (child is T t)
                    yield return t;

                if (child is ILogical logicalChild)
                {
                    foreach (var descendant in FindAllDescendants<T>(logicalChild))
                        yield return descendant;
                }
            }
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
