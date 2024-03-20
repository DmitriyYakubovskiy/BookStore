using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BookStore.Models
{
    public class BooksCollectionModel : INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public readonly ObservableCollection<BookModel> Books;

        public BooksCollectionModel(List<BookModel> Books)
        {
            this.Books = new ObservableCollection<BookModel>(Books);
            OnPropertyChanged(NotifyCollectionChangedAction.Add, new[] { Books });
        }

        public BooksCollectionModel() : this(new List<BookModel>()) { }

        public void ResetBooks(List<BookModel> BookModels)
        {
            Books.Clear();
            OnPropertyChanged(NotifyCollectionChangedAction.Reset);
            foreach (var book in BookModels)
            {
                AddBook(book);
            }
        }

        public void AddBook(BookModel book)
        {
            Books.Add(book);
            OnPropertyChanged(NotifyCollectionChangedAction.Add, new[] { book });
        }

        public void DeleteBook(BookModel book)
        {
            Books.Remove(book);
            OnPropertyChanged(NotifyCollectionChangedAction.Remove, new[] { book });
        }

        public void EditBook(BookModel book)
        {
            var oldBook = Books.FirstOrDefault(x => x.Id == book.Id);
            if (oldBook == null) return;
            oldBook.Title = book.Title;
            oldBook.AuthorName = book.AuthorName;
            oldBook.Year = book.Year;
            oldBook.Genre = book.Genre;
            oldBook.CostPrice = book.CostPrice;
            oldBook.SellingPrice = book.SellingPrice;
            oldBook.ContinuationBookTitle = book.ContinuationBookTitle;
            oldBook.PublisherName = book.PublisherName;
            oldBook.PageCount = book.PageCount;
            oldBook.NumberOfSales = book.NumberOfSales;
            OnPropertyChanged(NotifyCollectionChangedAction.Add, new[] { book });
        }

        private void OnPropertyChanged(NotifyCollectionChangedAction action, IList changedItems)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItems));
        }

        private void OnPropertyChanged(NotifyCollectionChangedAction action)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action));
        }
    }
}
