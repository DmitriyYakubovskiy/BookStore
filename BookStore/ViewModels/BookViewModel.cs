using BookStore.Commands;
using BookStore.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System;

namespace BookStore.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
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
        public ICommand CancelCommand => cancelCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        private BooksCollectionModel booksCollection;
        private BookModel book;
        private Window window;
        private readonly Command okCommand;
        private readonly Command cancelCommand;

        public BookViewModel(Window window, BooksCollectionModel booksCollection, BookModel book)
        {
            this.book = book;
            this.window = window;
            this.booksCollection = booksCollection;
            okCommand = new DelegateCommand(_ => Ok());
            cancelCommand = new DelegateCommand(_ => Cancel());
        }

        public BookViewModel(Window window, BooksCollectionModel booksCollection) : this(window, booksCollection, new BookModel()) { }

        private void Ok()
        {
            if (!CanOk()) return;
            if (window == null) return;
            window.DialogResult = true;
            window.Close();
        }

        private bool CanOk()
        {
            if (book.Title?.Length < 3 || book.AuthorName?.Length < 3 || book.PublisherName?.Length < 3 || book.Genre?.Length < 3)
            {
                MessageBox.Show("The field length is less than three characters");
                return false;
            }
            if (book.ContinuationBookTitle != "")
            {
                if (book.ContinuationBookTitle == book.Title) return false;
                foreach (var item in booksCollection.Books)
                {
                    if (item.Title == book.ContinuationBookTitle) return true;
                }
                MessageBox.Show("There is no continuation of the book");
                return false;
            }
            if (book.AuthorName.Split(" ").Length != 2)
            {
                MessageBox.Show("Enter the author's first name and last name");
                return false;
            }
            if (book.Year > DateTime.Today.Year)
            {
                MessageBox.Show("Wrong year");
                return false;
            }
            if(book.CostPrice <= 0 || book.SellingPrice<=0)
            {
                MessageBox.Show("Wrong price");
                return false;
            }
            return true;
        }

        private void Cancel()
        {
            if (window == null) return;
            window.DialogResult = false;
            window.Close();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
