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

            DataContext =
                new AssessmentViewModel(student);
        }
    }
