using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CareerCounsellingApp;

public partial class AdminDashboardWindow : Window
{
        public AdminDashboardWindow()
        {
            InitializeComponent();
            var btn = this.FindControl<Button>("CategoryButton");

            btn.Click += (_, _) =>
            {
                new CategoryManagementWindow().Show();
            };
            var questionBtn = this.FindControl<Button>("QuestionButton");

            questionBtn.Click += (_, _) =>
            {
                new QuestionManagementWindow().Show();
            };
            var studentButton =
            this.FindControl<Button>("StudentButton");

            studentButton.Click += (_, _) =>
            {
                new StudentManagementWindow().Show();
            };
        var assessmentResultsButton =
    this.FindControl<Button>(
        "AssessmentResultsButton");

        assessmentResultsButton.Click += (_, _) =>
        {
            new AssessmentResultsWindow().Show();
        };
    }
    
}