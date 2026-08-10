using Avalonia.Controls;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp
{
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();
            DataContext = new ReportsViewModel();
        }
    }
}
