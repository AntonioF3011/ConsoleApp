using CRM.Library.Services;
using CRM.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CRM.MAUI.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        public ShopViewModel()
        {
            InventoryQuery = string.Empty;
        }

        private string inventoryQuery;
        public string InventoryQuery
        {
            set
            {
                inventoryQuery = value;
                NotifyPropertyChanged();
            }
            get { return inventoryQuery; }
        }
        public ProductViewModel SelectedProduct { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public List<ProductViewModel> Products
        {
            get
            {
                return ProductServiceProxy.Current.Products.Where(p => p != null)
                    .Where(p => p?.Name?.ToUpper()?.Contains(InventoryQuery.ToUpper()) ?? false)
                    .Select(p => new ProductViewModel(p)).ToList() ?? new List<ProductViewModel>();
            }
        }
        public void RefreshProducts()
        {
            InventoryQuery = string.Empty;
            NotifyPropertyChanged(nameof(Products));
        }

        public void Search()
        {
            NotifyPropertyChanged(nameof(Products));
        }
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void ViewProduct()
        {
            if (SelectedProduct?.Product == null)
            {
                return;
            }
            Shell.Current.GoToAsync($"//Description?productId={SelectedProduct.Product.Id}"); //this is sending the product Id 

        }



        public void AddToCart()
        {
            if (SelectedProduct?.Product == null)
            {
                return;
            }



            if (SelectedProduct?.Quantity < 1)
            {
                SelectedProduct.Quantity = 0;
                return; 
            }
            ShoppingCartService.Current.AddToCart(SelectedProduct.Product);

            NotifyPropertyChanged(nameof(Products));
            NotifyPropertyChanged(nameof(TotalPrice));
            NotifyPropertyChanged(nameof(CartContents));
        }


        //==================CART STUFF JUST FOR VIEWS========================//
        private ShoppingCart cart;
        public ShoppingCart Cart2
        {
            get { return ShoppingCartService.Current.Cart; }
            set
            {
                cart = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CartContents));
            }
        }
        public decimal TotalPrice
        {
            get
            {
                return Cart2.Contents?.Sum(p => p.Price ) ?? 0;
            }

        }
        public ObservableCollection<Products> CartContents
        {
            get
            {
                return ShoppingCartService.Current.Cart.Contents;
            }
        }

    }
}