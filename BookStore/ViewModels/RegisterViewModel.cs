using BookStore.Commands;
using BookStore.DataAccess.Contexts;
using BookStore.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System;
using System.Linq;

namespace BookStore.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
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
        public ICommand CancelCommand => cancelCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        private UserEntity userEntity;
        private Command cancelCommand;
        private Command okCommand;
        private Window window;

        public RegisterViewModel(Window window, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.window = window;
            okCommand = new DelegateCommand(_ => Ok());
            cancelCommand = new DelegateCommand(_ => Cancel());
            userEntity = new UserEntity();
        }

        private void Ok()
        {
            if (!CanOk()) return;
            using (var context = new BookStoreDbContext(configuration))
            {
                UserEntity user = context.Users.FirstOrDefault((x => x.Login == userEntity.Login));
                if (user == null)
                {
                    context.Add(userEntity);
                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("This login is busy!");
                }
                window.Close();
            }
        }

        private bool CanOk()
        {
            if (userEntity.Login?.Length < 3 || userEntity.Password?.Length < 3)
            {
                MessageBox.Show("The field length is less than three characters");
                return false;
            }
            return true;
        }

        private void Cancel()
        {
            window.Close();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
