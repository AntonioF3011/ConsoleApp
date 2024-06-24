using CRM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CRM.Library.Services
{
    public class ProductServiceProxy
    {
        private List<Products> products;

        private ProductServiceProxy()
        {
            products = new List<Products>()
            {
                new Products { Id = 1, Name = "MacBook Pro", Price = 1750.0M, Quantity = 2, Description = "Intel Core i7 2.6GHz 6-Core (9t" +
                "h Generation) 16GB 26" +
                "66MHz DDR4 RAM | 512GB SSD 16-inch 3072 x 1920 Retina Display AMD Radeon Pro 5300M GPU (4GB GDDR6) P" +
                "3 Color Gamut" },
                new Products { Id = 2, Name = "iPhone", Price = 1750.0M, Quantity = 4, Description = "Dynamic island. A magical wa" +
                "y to interact with the iPhone. A16 Bionic chip with 5-core GPU" }
            };
        }

        private static ProductServiceProxy? instance;
        private static readonly object instanceLock = new object();

        public static ProductServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductServiceProxy();
                    }

                    return instance;
                }
            }
        }

        public ReadOnlyCollection<Products> Products
        {
            get
            {
                return products.AsReadOnly();
            }
        }

        private int NextId
        {
            get
            {
                if (!products.Any())
                {
                    return 1;
                }

                return products.Max(p => p.Id) + 1;
            }
        }


        public Products AddOrUpdate(Products p)
        {
            if (p == null)
            {
                throw new ArgumentNullException(nameof(p));
            }

            if (p.Id == 0)
            {
                p.Id = NextId;
                products.Add(p);
            }
            else
            {
                var existingProduct = products.FirstOrDefault(pr => pr.Id == p.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = p.Name;
                    existingProduct.Description = p.Description;
                    existingProduct.Quantity = p.Quantity;
                    existingProduct.Price = p.Price;
                }
                else
                {
                    products.Add(p);
                }
            }

            return p;
        }

        public void Delete(int id)
        {
            var productToDelete = products.FirstOrDefault(p => p.Id == id);
            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
        }
    }
}
