using CRM.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace CRM.Library.Services
{
    public class ShoppingCartService
    {
        private static ShoppingCartService? instance;
        private static object instanceLock = new object();

        private ObservableCollection<ShoppingCart> carts = new ObservableCollection<ShoppingCart>();

        public ShoppingCart Cart
        {
            get
            {
                if (carts == null || !carts.Any())
                {
                    var newCart = new ShoppingCart { Id = 1, Contents = new ObservableCollection<Products>() };
                    carts.Add(newCart);
                    return newCart;
                }
                return carts.First();
            }
        }

        private ShoppingCartService() { }

        public static ShoppingCartService Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ShoppingCartService();
                    }
                }
                return instance;
            }
        }

        public void AddToCart(Products newProduct)
        {
            if (newProduct == null) return;

            var existingProduct = Cart.Contents.FirstOrDefault(p => p.Id == newProduct.Id);
            var inventoryProduct = ProductServiceProxy.Current.Products.FirstOrDefault(p => p.Id == newProduct.Id);

            if (inventoryProduct == null || inventoryProduct.Quantity < 1)
            {
                
                return;
            }

            

            Cart.Contents.Add(newProduct);
            --inventoryProduct.Quantity;
        }

        public void RemoveFromCart(int productId)
        {
            var productToRemove = Cart?.Contents?.FirstOrDefault(p => p.Id == productId);
            if (productToRemove != null)
            {
                var inventoryProduct = ProductServiceProxy.Current.Products.FirstOrDefault(p => p.Id == productId);
                if (inventoryProduct != null)
                {
                    inventoryProduct.Quantity += productToRemove.Quantity;
                }
                Cart.Contents.Remove(productToRemove);
            }
        }
    }
}
