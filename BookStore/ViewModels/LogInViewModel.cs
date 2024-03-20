using BookStore.Commands;
using BookStore.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BookStore.DataAccess.Contexts;
using System.Windows;
using BookStore.Views;
using System.Linq;

namespace BookStore.ViewModels
{
    public class LogInViewModel:INotifyPropertyChanged
    {
        public UserEntity UserEntity
        {
            get => userEntity;
            set
            {
                userEntity = value;
                OnPropertyChanged();
            }
        }
        public IConfiguration configuration;
        public ICommand OkCommand => okCommand;
        public ICommand RegCommand=> regCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        private UserEntity userEntity;
        private Command okCommand;
        private Command regCommand;

        public LogInViewModel()
        {
            okCommand = new DelegateCommand(_ => Ok());
            regCommand = new DelegateCommand(_ => Reg());
            userEntity = new UserEntity();
        }

        private void Ok()
        {
            configuration = BuildConfiguration();
            using (var context = new BookStoreDbContext(configuration))
            {
                UserEntity user = context.Users.FirstOrDefault(x => x.Login == userEntity.Login);
                if (user == null)
                {
                    MessageBox.Show("The user was not found!");
                    return;
                }
                if (user.Password != userEntity.Password)
                {
                    MessageBox.Show("Invalid password");
                    return;
                }
                userEntity = user;
            }
            var mainWindow = new MainWindow();
            var viewModel = new MainWindowViewModel(mainWindow,configuration,userEntity);
            mainWindow.DataContext = viewModel;
            if (mainWindow.ShowDialog() != true) return;
            userEntity= new UserEntity(); 
            OnPropertyChanged(nameof(UserEntity));
        }

        private void Reg()
        {
            configuration = BuildConfiguration();
            var window = new RegisterView();
            var viewModel = new RegisterViewModel(window,configuration);
            window.DataContext = viewModel;
            window.Show();
        }

        private IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
