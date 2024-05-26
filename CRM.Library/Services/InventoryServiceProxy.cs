using CRM.Models;
using System.Collections.ObjectModel;

namespace CRM.Library.Services
{
    public class InventoryServiceProxy
    {
        private InventoryServiceProxy()
        {
            items = new List<InventoryItem>();
        }

        private static InventoryServiceProxy? instance;
        private static object instanceLock = new object();
        public static InventoryServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new InventoryServiceProxy();
                    }
                }

                return instance;
            }
        }

        private List<InventoryItem>? items;
        public ReadOnlyCollection<InventoryItem>? Items
        {
            get
            {
                return items?.AsReadOnly();
            }
        }

        public int LastId
        {
            get
            {
                if (items?.Any() ?? false)
                {
                    return items?.Select(c => c.Id)?.Max() ?? 0;
                }
                return 0;
            }
        }

        public InventoryItem? AddOrUpdate(InventoryItem item)
        {
            if (items == null)
            {
                return null;
            }

            var existingItem = items.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Name = item.Name;
                existingItem.Description = item.Description;
                existingItem.Price = item.Price;
                existingItem.Quantity = item.Quantity;
            }
            else
            {
                item.Id = LastId + 1;
                items.Add(item);
            }

            return item;
        }

        public void Delete(int id)
        {
            if (items == null)
            {
                return;
            }
            var itemToDelete = items.FirstOrDefault(i => i.Id == id);

            if (itemToDelete != null)
            {
                items.Remove(itemToDelete);
            }
        }
    }
}
