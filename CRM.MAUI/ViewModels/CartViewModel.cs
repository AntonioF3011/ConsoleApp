using CRM.Library.Services;
using CRM.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CRM.MAUI.ViewModels
{
    public class CartViewModel : INotifyPropertyChanged
    {
        private ShoppingCart cart;

        public ShoppingCart Cart
        {
            get { return cart; }
            set
            {
                if (cart != value)
                {
                    cart = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(CartContents));
                    NotifyPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public ObservableCollection<Products> CartContents
        {
            get { return Cart.Contents; }
        }

        public CartViewModel()
        {
            Cart = ShoppingCartService.Current.Cart;
            Cart.Contents.CollectionChanged += (s, e) =>
            {
                NotifyPropertyChanged(nameof(CartContents));
                NotifyPropertyChanged(nameof(TotalPrice));
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public decimal TotalPrice
        {
            get
            {
                return Cart.Contents?.Sum(p => p.Price) ?? 0;
            }
        }
    }
}
