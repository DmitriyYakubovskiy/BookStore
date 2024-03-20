using BookStore.Commands;
using BookStore.DataAccess.Contexts;
using BookStore.DataAccess.Entities;
using BookStore.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System;

namespace BookStore.ViewModels
{
    public class DiscountsViewModel:INotifyPropertyChanged
    {
        private Window window;
        private BookModel bookModel;
        private DiscountEntity discountEntity;
        private readonly DiscountCollection discountsCollection;
        private readonly Command okCommand;
        private readonly Command addCommand;
        private readonly Command deleteCommand;
        private IConfiguration configuration;

        public BookModel Book
        {
            get => bookModel;
            set
            {
                bookModel = value;
                OnPropertyChanged(nameof(Book));
            }
        }
        public DiscountEntity Discount
        {
            get => discountEntity;
            set
            {
                discountEntity = value;
                OnPropertyChanged(nameof(Discount));
            }
        }
        public ICommand OkCommand => okCommand;
        public ICommand AddCommand => addCommand;
        public ICommand DeleteCommand => deleteCommand;
        public IReadOnlyCollection<DiscountEntity> Discounts => discountsCollection.Discounts;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DiscountsViewModel(Window window,IConfiguration configuration)
        {
            this.window = window;
            this.configuration = configuration;
            this.discountEntity=new DiscountEntity();
            discountEntity=new DiscountEntity();
            bookModel= new BookModel(); 
            okCommand = new DelegateCommand(_ => Ok());
            addCommand = new DelegateCommand(_ => Add());
            deleteCommand = new GenericCommand<DiscountEntity>(Delete);

            discountsCollection = LoadDiscounts();
            discountsCollection.CollectionChanged += (_, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    OnPropertyChanged(nameof(Discounts));
                }
            };
        }

        private void Ok()
        {
            if (window == null) return;
            window.DialogResult = true;
            window.Close();
        }

        private void Add()
        {
            try
            {
                if (Book.Title != "") discountEntity.Title = Book.Title;
                if (Book.AuthorName != "") discountEntity.AuthorName = Book.AuthorName;
                if(Book.PublisherName != "") discountEntity.PublisherName = Book.PublisherName;
                if(Book.Genre != "") discountEntity.Genre = Book.Genre;
                if (discountEntity.Name== "")
                {
                    MessageBox.Show("Empty discount name!");
                    return;
                }
                if (discountEntity.Percent <0)
                {
                    MessageBox.Show("Incorrect percent!");
                    return;
                }
                discountsCollection.AddDiscount(discountEntity);
                using (var context = new BookStoreDbContext(configuration))
                {
                    context.Add(discountEntity);
                    context.SaveChanges();
                }
                OnPropertyChanged(nameof(Discounts));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Delete(DiscountEntity discountEntity)
        {
            discountsCollection.DeleteDiscount(discountEntity);
            using (var context = new BookStoreDbContext(configuration))
            {
                context.Remove(discountEntity);
                context.SaveChanges();
            }
            OnPropertyChanged(nameof(Discounts));
        }

        private DiscountCollection LoadDiscounts()
        {
            using(var context=new BookStoreDbContext(configuration))
            {
                return new DiscountCollection(context.Discounts.ToList());
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
