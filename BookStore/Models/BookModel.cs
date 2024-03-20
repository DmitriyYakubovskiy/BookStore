using BookStore.DataAccess.Entities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookStore.Models
{
    public class BookModel : INotifyPropertyChanged, ICloneable
    {
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }
        public string AuthorName
        {
            get => authorName;
            set
            {
                authorName = value;
                OnPropertyChanged();
            }
        }
        public string PublisherName
        {
            get => publisherName;
            set
            {
                publisherName = value;
                OnPropertyChanged();
            }
        }
        public string Genre
        {
            get => genre;
            set
            {
                genre = value;
                OnPropertyChanged();
            }
        }
        public int PageCount
        {
            get => pageCount;
            set
            {
                pageCount = value;
                OnPropertyChanged();
            }
        }
        public int Year
        {
            get => year;
            set
            {
                year = value;
                OnPropertyChanged();
            }
        }
        public float CostPrice
        {
            get => costPrice;
            set
            {
                costPrice = value;
                OnPropertyChanged();
            }
        }
        public float SellingPrice
        {
            get => sellingPrice;
            set
            {
                sellingPrice = value;
                OnPropertyChanged();
            }
        }
        public int NumberOfSales
        {
            get => numberOfSales;
            set
            {
                numberOfSales = value;
                OnPropertyChanged();
            }
        }
        public string ContinuationBookTitle
        {
            get => continuationBookName;
            set
            {
                continuationBookName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private int id;
        private string title = "";
        private string authorName = "";
        private string publisherName = "";
        private string genre = "";
        private int pageCount;
        private int year;
        private float costPrice;
        private float sellingPrice;
        private int numberOfSales = 0;
        private string continuationBookName = "";
        
        public BookModel() { }

        public BookModel(BookEntity book)
        {
            if (book != null)
            {
                Title = book.Title;
                AuthorName = book.Author.FirstName + " " + book.Author.LastName;
                PublisherName = book.Publisher.Name;
                Genre = book.Genre.Name;
                PageCount = book.PageCount;
                Year = book.Year;
                CostPrice = book.CostPrice;
                SellingPrice = book.SellingPrice;
                NumberOfSales = book.NumberOfSales;
                ContinuationBookTitle = book.ContinuationBook != null ? book.ContinuationBook.Title : "";
            }
        }

        public object Clone()
        {
            return new BookModel
            {
                Id = id,
                Title = title,
                AuthorName = authorName,
                PublisherName = publisherName,
                Genre = genre,
                PageCount = pageCount,
                Year = year,
                CostPrice = costPrice,
                SellingPrice = sellingPrice,
                NumberOfSales = numberOfSales,
                ContinuationBookTitle = continuationBookName,
            };
        }

        public override string ToString()
        {
            return $"{title}\t{authorName}\t{publisherName}\t{genre}\t{pageCount}\t{year}\t{costPrice}\t{sellingPrice}\t{NumberOfSales}\t{continuationBookName}";
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
