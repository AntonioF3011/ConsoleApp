using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CRM.Library.Services;
using CRM.Models;

namespace CRM.MAUI.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private Products? product;

        public Products? Product
        {
            get { return product; }
            set
            {
                if (product != value)
                {
                    product = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(Name));
                    NotifyPropertyChanged(nameof(Description));
                    NotifyPropertyChanged(nameof(Id));
                    NotifyPropertyChanged(nameof(Quantity));
                    NotifyPropertyChanged(nameof(DisplayPrice));
                    NotifyPropertyChanged(nameof(Price));
                }
            }
        }

        public string? Name
        {
            get { return Product?.Name ?? string.Empty; }
            set
            {
                if (Product != null && Product.Name != value)
                {
                    Product.Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string? Description
        {
            get { return Product?.Description ?? string.Empty; }
            set
            {
                if (Product != null && Product.Description != value)
                {
                    Product.Description = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Id
        {
            get { return Product?.Id ?? 0; }
            set
            {
                if (Product != null && Product.Id != value)
                {
                    Product.Id = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Quantity
        {
            get { return Product?.Quantity ?? 0; }
            set
            {
                if (Product != null && Product.Quantity != value)
                {
                    Product.Quantity = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string DisplayPrice
        {
            get
            {
                if (Product == null) { return string.Empty; }
                return $"{Product.Price:C}";
            }
        }

        public decimal Price
        {
            get { return Product?.Price ?? 0; }
            set
            {
                if (Product != null && Product.Price != value)
                {
                    Product.Price = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand? EditCommand { get; private set; }
        public ICommand? AddToCartCommand { get; private set; }
        public ICommand? DeleteCommand { get; private set; }

        public ProductViewModel()
        {
            Product = new Products();
            SetupCommands();
        }

        public ProductViewModel(int id)
        {
            Product = ProductServiceProxy.Current?.Products?.FirstOrDefault(p => p.Id == id);
            if (Product == null)
            {
                Product = new Products();
            }
            SetupCommands();
        }

        public ProductViewModel(Products p)
        {
            Product = p;
            SetupCommands();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExecuteEdit(ProductViewModel? p)
        {
            if (p?.Product == null)
            {
                return;
            }
            Shell.Current.GoToAsync($"//Product?productId={p.Product.Id}");
        }
        private void ExecuteAddToCart(ProductViewModel? p)
        {
            if (p?.Product == null)
            {
                return;
            }
            ShoppingCartService.Current.AddToCart(p.Product);
            NotifyPropertyChanged(nameof(Products));
        }

        private void ExecuteDelete(int? id)
        {
            if (id == null)
            {
                return;
            }
            ProductServiceProxy.Current.Delete(id ?? 0);
        }

        public void Add()
        {
            if (Product == null)
            {
                return;
            }
            ProductServiceProxy.Current.AddOrUpdate(Product);
        }

        public void SetupCommands()
        {
            EditCommand = new Command((p) => ExecuteEdit(p as ProductViewModel));
            DeleteCommand = new Command((p) => ExecuteDelete((p as ProductViewModel)?.Product?.Id));
            AddToCartCommand = new Command((p) => ExecuteAddToCart((p as ProductViewModel))); 
        }

        public string? Display
        {
            get { return ToString(); }
        }
    }
}
