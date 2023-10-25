using PESEL_Database_Tests.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PESEL_Database_Tests
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Window mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}