using BookStore.DataAccess.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BookStore.Models
{
    public class DiscountCollection : INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public readonly ObservableCollection<DiscountEntity> Discounts;

        public DiscountCollection(List<DiscountEntity> Discounts)
        {
            this.Discounts = new ObservableCollection<DiscountEntity>(Discounts);
            OnPropertyChanged(NotifyCollectionChangedAction.Add, new[] { Discounts });
        }

        public DiscountCollection() : this(new List<DiscountEntity>()) { }


        public void AddDiscount(DiscountEntity discount)
        {
            Discounts.Add(discount);
            OnPropertyChanged(NotifyCollectionChangedAction.Add, new[] { discount });
        }

        public void DeleteDiscount(DiscountEntity discount)
        {
            Discounts.Remove(discount);
            OnPropertyChanged(NotifyCollectionChangedAction.Remove, new[] { discount });
        }

        private void OnPropertyChanged(NotifyCollectionChangedAction action, IList changedItems)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItems));
        }
    }
}
