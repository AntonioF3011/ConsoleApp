using CRM.Library.Services;

namespace CRM.Models
{
    public class InventoryItem
    {
        public int Id { get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int? Quantity { get; set; }

        public override string ToString()
        {
            return $"ID:[{Id}] Name:{Name} || Descrp: {Description} ||Price: ${Price} - Amount left: {Quantity}";
        }
        
    }
}

