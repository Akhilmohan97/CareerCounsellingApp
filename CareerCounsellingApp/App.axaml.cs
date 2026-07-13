using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CareerCounsellingApp.Data;
using CareerCounsellingApp.ViewModels;
using CareerCounsellingApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CareerCounsellingApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            using var db = new AppDbContext();

            db.Database.Migrate();
            DbInitializer.Seed(db);
            base.OnFrameworkInitializationCompleted();
            AppServices.Provider = DependencyInjection.DependencyInjection.Configure();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            
        }
    }
}