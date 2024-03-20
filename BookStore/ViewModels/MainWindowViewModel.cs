using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BookStore.DataAccess.Entities;
using BookStore.Commands;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BookStore.DataAccess.Contexts;
using BookStore.Views;
using System.Collections.ObjectModel;
using System;

namespace BookStore.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public BookModel BooksFilterParam
        {
            get => booksFilterParam;
            set
            {
                if (booksFilterParam == null || booksFilterParam == value) return;
                booksFilterParam = value;
                OnPropertyChanged();
            }
        }
        public UserEntity UserEntity => userEntity;
        public IConfiguration configuration;
        public ICommand ExitCommand => exitCommand;
        public ICommand ShowDiscountsCommand => showDiscountsCommand;
        public ICommand AddBookCommand => addBookCommand;
        public ICommand EditBookCommand => editBookCommand;
        public ICommand DeleteBookCommand => deleteBookCommand;
        public ICommand BuyBookCommand => buyBookCommand;
        public ICommand ShowAllInfoCommand => showAllInfoCommand;
        public ICommand SortPriceMinCommand => sortPriceMinCommand;
        public ICommand SortPriceMaxCommand=> sortPriceMaxCommand;
        public ICommand SortNewsCommand => sortNewsCommand;
        public ICommand SortPopularBookCommand => sortPopularBookCommand;
        public ICommand SortPopularAuthorsCommand => sortPopularAuthorsCommand;
        public ICommand SortPopularGenresCommand => sortPopularGenresCommand;
        public ICommand SearchBookCommand => searchBookCommand;
        public ICommand ClearSearchCommand => clearSearchCommand;
        public IReadOnlyCollection<BookModel> Books => booksCollection.Books;
        public IReadOnlyCollection<int> TableHeader => tableHeader;

        public event PropertyChangedEventHandler? PropertyChanged;

        private BookModel booksFilterParam = new BookModel() { Genre = null!, Title = null!, AuthorName = null! };
        private UserEntity userEntity;
        private Window window;
        private readonly BooksCollectionModel booksCollection;
        private readonly ObservableCollection<int> tableHeader = new ObservableCollection<int>() { 1 };
        private readonly Command exitCommand;
        private readonly Command showDiscountsCommand;
        private readonly Command addBookCommand;
        private readonly Command editBookCommand;
        private readonly Command deleteBookCommand;
        private readonly Command buyBookCommand;
        private readonly Command showAllInfoCommand;
        private readonly Command sortPriceMinCommand;
        private readonly Command sortPriceMaxCommand;
        private readonly Command sortNewsCommand;
        private readonly Command sortPopularBookCommand;
        private readonly Command sortPopularAuthorsCommand;
        private readonly Command sortPopularGenresCommand;
        private readonly Command searchBookCommand;
        private readonly Command clearSearchCommand;

        public MainWindowViewModel(Window window,IConfiguration configuration, UserEntity userEntity)
        {
            this.window = window;
            this.configuration = configuration;
            this.userEntity = userEntity;
            exitCommand = new DelegateCommand(_ => Exit());
            showDiscountsCommand = new DelegateCommand(_ => ShowDiscounts(), _=> CanInteractWithBook());
            addBookCommand = new DelegateCommand(_ => OpenAddBookWindow(), _ => CanInteractWithBook());
            sortPriceMinCommand = new DelegateCommand(_ => SortPriceMin());
            sortPriceMaxCommand = new DelegateCommand(_ => SortPriceMax());
            sortNewsCommand = new DelegateCommand(_ => SortNews());
            sortPopularBookCommand = new DelegateCommand(_ => SortPopularBook());
            sortPopularAuthorsCommand = new DelegateCommand(_ => SortPopularAuthors());
            sortPopularGenresCommand = new DelegateCommand(_ => SortPopularGenres());
            searchBookCommand = new DelegateCommand(_ => Search());
            clearSearchCommand = new DelegateCommand(_ => ClearSearch());

            editBookCommand = new GenericCommand<BookModel>(OpenEditBookWindow, _ => CanInteractWithBook());
            deleteBookCommand = new GenericCommand<BookModel>(DeleteBook, _ => CanInteractWithBook());
            buyBookCommand = new GenericCommand<BookModel>(BuyBook);
            showAllInfoCommand = new GenericCommand<BookModel>(ShowAllInfo);

            booksCollection = LoadData();
            booksCollection.CollectionChanged += (_, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    OnPropertyChanged(nameof(Books));
                }
            };
        }

        private void Exit()
        {
            window.DialogResult = true;
            window.Close();
        }

        private void ShowDiscounts()
        {
            var window = new DiscountsView(this.window);
            var viewModel = new DiscountsViewModel(window, configuration);
            window.DataContext = viewModel;
            if (window.ShowDialog() != true) return;
            ClearSearch();
        }

        private void OpenAddBookWindow()
        {
            try
            {
                var window = new BookView(this.window);
                var viewModel = new BookViewModel(window, booksCollection);
                window.DataContext = viewModel;
                if (window.ShowDialog() != true) return;
                if (booksCollection.Books.Count > 0) viewModel.Book.Id = booksCollection.Books.OrderByDescending(l => l.Id).First().Id + 1;
                else viewModel.Book.Id = 1;
                booksCollection.AddBook(viewModel.Book);
                UpdateAuthor(viewModel.Book);
                UpdateGenre(viewModel.Book);
                UpdatePublisher(viewModel.Book);
                PushData(viewModel.Book);
                OnPropertyChanged(nameof(Books));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenEditBookWindow(BookModel book)
        {
            try
            {
                var window = new BookView(this.window);
                var viewModel = new BookViewModel(window, booksCollection, book.Clone() as BookModel);
                window.DataContext = viewModel;
                if (window.ShowDialog() != true) return;
                booksCollection.EditBook(viewModel.Book);
                UpdateAuthor(viewModel.Book);
                UpdateGenre(viewModel.Book);
                UpdatePublisher(viewModel.Book);
                PushData(viewModel.Book);
                OnPropertyChanged(nameof(Books));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteBook(BookModel book)
        {
            DeleteBookEntity(book);
            booksCollection.DeleteBook(book);
            OnPropertyChanged(nameof(Books));
        }

        private void BuyBook(BookModel bookModel)
        {

            bookModel.NumberOfSales++;
            using (var context = new BookStoreDbContext(configuration))
            {
                var book = context.Books.Where(x => x.Id == bookModel.Id).FirstOrDefault();
                book.NumberOfSales++;
                context.SaveChanges();
                MessageBox.Show($"Книга '{bookModel.Title}' куплена!");
            }
        }

        private void ShowAllInfo(BookModel bookModel)
        {
            var window = new ShowBookInfoView(this.window);
            var viewModel = new ShowBookInfoViewModel(window,bookModel.Clone() as BookModel);
            window.DataContext = viewModel;
            if (window.ShowDialog() != true) return;
        }

        private void SortPriceMin()
        {
            List<BookModel> query = LoadData().Books.OrderBy(x => x.SellingPrice).ToList();
            booksCollection.ResetBooks(query);
            OnPropertyChanged(nameof(Books));
        }

        private void SortPriceMax()
        {
            List<BookModel> query = LoadData().Books.OrderByDescending(x => x.SellingPrice).ToList();
            booksCollection.ResetBooks(query);
            OnPropertyChanged(nameof(Books));
        }

        private void SortNews()
        {
            List<BookModel> query = LoadData().Books.OrderByDescending(x => x.Year).ToList();
            booksCollection.ResetBooks(query);
            OnPropertyChanged(nameof(Books));
        }

        private void SortPopularBook()
        {
            List<BookModel> query = LoadData().Books.OrderByDescending(x => x.NumberOfSales).ToList();
            booksCollection.ResetBooks(query);
            OnPropertyChanged(nameof(Books));
        }
        private void SortPopularAuthors()
        {
            List<BookModel> query = LoadData().Books.OrderByDescending(b => b.NumberOfSales)
                      .GroupBy(b => b.AuthorName)
                      .OrderByDescending(g => g.Sum(b => b.NumberOfSales))
                      .SelectMany(g => g).ToList();
            booksCollection.ResetBooks(query);
            OnPropertyChanged(nameof(Books));
        }

        private void SortPopularGenres()
        {
            List<BookModel> query = LoadData().Books.OrderByDescending(b => b.NumberOfSales)
          .GroupBy(b => b.Genre)
          .OrderByDescending(g => g.Sum(b => b.NumberOfSales))
          .SelectMany(g => g).ToList();
            booksCollection.ResetBooks(query);
            OnPropertyChanged(nameof(Books));
        }

        private void Search()
        {
            List<BookModel> query = LoadData().Books.ToList();
            if (booksFilterParam.Title != "" && booksFilterParam.Title != null) query = query.Where(x => x.Title.ToLower() == booksFilterParam.Title.ToLower()).ToList();
            if (booksFilterParam.AuthorName != "" && booksFilterParam.AuthorName != null)
            {
                if (booksFilterParam.AuthorName.Split(" ").Length == 1)
                {
                    query = query.Where(x => x.AuthorName.ToLower().Split(" ")[0] == booksFilterParam.AuthorName.ToLower() || x.AuthorName.ToLower().Split(" ")[1] == booksFilterParam.AuthorName.ToLower()).ToList();
                }
                if (booksFilterParam.AuthorName.Split(" ").Length == 2)
                {
                    query = query.Where(x => x.AuthorName.ToLower().Split(" ")[0] + " " + x.AuthorName.ToLower().Split(" ")[1] == booksFilterParam.AuthorName.ToLower()
                    || x.AuthorName.ToLower().Split(" ")[1] + " " + x.AuthorName.ToLower().Split(" ")[0] == booksFilterParam.AuthorName.ToLower()).ToList();
                }
            }
            if (booksFilterParam.Genre != "" && booksFilterParam.Genre != null) query = query.Where(x => x.Genre.ToLower() == booksFilterParam.Genre.ToLower()).ToList();
            booksCollection.ResetBooks(query);
            OnPropertyChanged(nameof(Books));
        }

        private void ClearSearch()
        {
            booksFilterParam = new BookModel() { Title = null!, AuthorName = null!, Genre = null! };
            OnPropertyChanged(nameof(booksFilterParam));
            booksCollection.ResetBooks(LoadData().Books.ToList());
            OnPropertyChanged(nameof(Books));
        }

        private bool CanInteractWithBook()
        {
            if (userEntity.IsAdmin==false) return false;
            if (booksFilterParam.Title != null) return false;
            if (booksFilterParam.Genre != null) return false;
            if (booksFilterParam.AuthorName != null) return false;
            return true;
        }

        private void DeleteBookEntity(BookModel bookModel)
        {
            using (var context = new BookStoreDbContext(configuration))
            {
                var book = context.Books.Where(x => x.Id == bookModel.Id).FirstOrDefault();
                context.Remove(book);
                context.SaveChanges();
            }
        }

        private void UpdateAuthor(BookModel book)
        {
            using (var context = new BookStoreDbContext(configuration))
            {
                bool check = true;
                foreach (var author in context.Authors)
                {
                    if (book.AuthorName.ToLower() == author.FirstName.ToLower() + " " + author.LastName.ToLower())
                    {
                        check = false;
                        break;
                    }
                }
                if (check) context.Authors.Add(new AuthorEntity { FirstName = book.AuthorName.Split(" ")[0], LastName = book.AuthorName.Split(" ")[1] });

                context.SaveChanges();
            }
        }

        private void UpdatePublisher(BookModel book)
        {
            using (var context = new BookStoreDbContext(configuration))
            {
                bool check = true;
                foreach (var publisher in context.Publishers)
                {
                    if (book.PublisherName.ToLower() == publisher.Name.ToLower())
                    {
                        check = false;
                        break;
                    }
                }
                if (check) context.Publishers.Add(new PublisherEntity { Name = book.PublisherName });

                context.SaveChanges();
            }
        }

        private void UpdateGenre(BookModel book)
        {
            using (var context = new BookStoreDbContext(configuration))
            {
                bool check = true;
                foreach (var genre in context.Genres)
                {
                    if (book.Genre.ToLower() == genre.Name.ToLower())
                    {
                        check = false;
                        break;
                    }
                }
                if (check) context.Genres.Add(new GenreEntity { Name = book.Genre });

                context.SaveChanges();
            }
        }

        private void PushData(BookModel bookModel)
        {
            using (var context = new BookStoreDbContext(configuration))
            {
                var book = context.Books.Where(x => x.Id == bookModel.Id).FirstOrDefault();
                bool check = true;

                if (book == null)
                {
                    book = new BookEntity();
                    check = false;
                }
                book.Title = bookModel.Title;
                book.AuthorId = context.Authors.Where(x => x.FirstName.ToLower() + " " + x.LastName.ToLower() == bookModel.AuthorName.ToLower()).FirstOrDefault().Id;
                book.Year = bookModel.Year;
                book.GenreId = context.Genres.Where(x => x.Name.ToLower() == bookModel.Genre.ToLower()).FirstOrDefault().Id;
                book.CostPrice = bookModel.CostPrice;
                book.SellingPrice = bookModel.SellingPrice;
                book.ContinuationBookId = context.Books.Where(x => x.Title.ToLower() == bookModel.ContinuationBookTitle.ToLower()).FirstOrDefault()?.Id;
                book.PublisherId = context.Publishers.Where(x => x.Name.ToLower() == bookModel.PublisherName.ToLower()).FirstOrDefault().Id;
                book.PageCount = bookModel.PageCount;
                book.NumberOfSales = bookModel.NumberOfSales;

                if (!check) context.Books.Add(book);

                context.SaveChanges();
            }
        }

        private BooksCollectionModel LoadData()
        {
            var context = new BookStoreDbContext(configuration);
            var items = new List<BookModel>();
            foreach (var book in context.Books.Include(x => x.Author).Include(x => x.Publisher).Include(x => x.Genre))
            {
                items.Add(new BookModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    AuthorName = book.Author.FirstName + " " + book.Author.LastName,
                    PublisherName = book.Publisher.Name,
                    Genre = book.Genre.Name,
                    PageCount = book.PageCount,
                    Year = book.Year,
                    CostPrice = book.CostPrice,
                    SellingPrice = book.SellingPrice,
                    ContinuationBookTitle = book.ContinuationBook != null ? book.ContinuationBook.Title : "",
                    NumberOfSales = book.NumberOfSales,
                });
            }

            var discounts =context.Discounts;
            foreach(var discount in discounts)
            {
                var books=new List<BookModel>();
                if(discount.Title != "") books = items.Where(x => x.Title == discount.Title).ToList();
                if (discount.AuthorName != "") books = items.Where(x => x.AuthorName == discount.AuthorName).ToList();
                if (discount.PublisherName != "") books = items.Where(x => x.PublisherName == discount.PublisherName).ToList();
                if (discount.Genre != "") books = items.Where(x => x.Genre == discount.Genre).ToList();
                books.ForEach(x => x.SellingPrice *= 1-(discount.Percent / 100f));
            }

            return new BooksCollectionModel(items);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
