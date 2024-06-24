using CRM.Library.Services;
using System.Windows.Input;

namespace CRM.Models
{
    public class Products
    {
       
        public int Id { get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        
        public Products()
        {

        }
        public override string ToString()
        {
            return $"ID:[{Id}] Name:{Name} || Descrp: {Description} ||Price: ${Price} - Amount left: {Quantity}";
        }
        
    }
}

