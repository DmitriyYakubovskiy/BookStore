using BookStore.ViewModels;
using BookStore.Views;
using System.Windows;

namespace BookStore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() { ShutdownMode = ShutdownMode.OnMainWindowClose; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var logInView = new LogInView();
            logInView.DataContext = new LogInViewModel();
            logInView.Show();
        }
    }
}
