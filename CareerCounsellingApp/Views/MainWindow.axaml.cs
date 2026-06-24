using Avalonia.Controls;
using CareerCounsellingApp.Services;
using CareerCounsellingApp.ViewModels;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Opened += MainWindow_Opened;
        }

        private async void MainWindow_Opened(object? sender, System.EventArgs e)
        {
#if !DEBUG
            await Task.Delay(5000);

            var updateService = new UpdateService();

            var update = await updateService.CheckForUpdatesAsync();

            if (update != null)
            {
                var window = new UpdateWindow(update);
                await window.ShowDialog(this);
            } 
#endif
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

