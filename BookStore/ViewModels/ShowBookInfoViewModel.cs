using BookStore.Commands;
using BookStore.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModels
{
    public class ShowBookInfoViewModel : INotifyPropertyChanged
    {
        public BookModel Book
        {
            get => book;
            set
            {
                if (book == null || book == value) return;
                book = value;
                OnPropertyChanged();
            }
        }
        public ICommand OkCommand => okCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        private BookModel book;
        private Window window;
        private readonly Command okCommand;

        public ShowBookInfoViewModel(Window window, BookModel book)
        {
            this.window = window;
            this.book = book;
            okCommand = new DelegateCommand(_ =>Ok());
        }

        private void Ok()
        {
            if (window == null) return;
            window.DialogResult = true;
            window.Close();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
