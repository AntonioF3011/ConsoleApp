using CRM.Library.Services;
using System.Collections.ObjectModel;

namespace CRM.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public ObservableCollection<Products> Contents { get; set; }

        public ShoppingCart()
        {
            Contents = new ObservableCollection<Products>();
        }
       
    }
}
