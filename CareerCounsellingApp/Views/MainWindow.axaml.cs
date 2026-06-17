using Avalonia.Controls;
using CareerCounsellingApp.ViewModels;

namespace CareerCounsellingApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnDataContextChanged(System.EventArgs e)
        {
            base.OnDataContextChanged(e);
            
            // Pass the window reference to the ViewModel so it can close itself after login
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.LoginWindow = this;
            }
        }
    }
}

