using CRM.Library.Services;
using CRM.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp.Helpers
{
    public class ShopCart
    {
        private readonly InventoryServiceProxy inventorySvc;

        public ShopCart()
        {
            inventorySvc = InventoryServiceProxy.Current;
        }
        public void Shop()
        {
            var cart = new List<InventoryItem>();
            
            while (true)
            {
                Console.WriteLine("Shop:");

                Console.WriteLine(@"//==========================\\");
                Console.WriteLine("||1. Add item to cart       ||");
                Console.WriteLine("||2. Remove item from cart  ||");
                Console.WriteLine("||3. Checkout               ||");
                Console.WriteLine("||4. Back to Menu           ||");
                Console.WriteLine(@"\\==========================//");
                Console.Write("Choose an option: ");
                Console.WriteLine();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddToCart(cart);
                        break;
                    case "2":
                        RemoveFromCart(cart);
                        break;
                    case "3":
                        Checkout(cart);
                        return;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
        private void AddToCart(List<InventoryItem> cart)
        {
            Console.Write("Enter item ID to add to cart: ");
            var id = int.Parse(Console.ReadLine() ?? "0");

            var item = inventorySvc.Items?.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("Item not found.");
                return;
            }

            Console.Write("Enter quantity to add to cart: ");
            var quantity = int.Parse(Console.ReadLine() ?? "0");

            if (quantity > item.Quantity)
            {
                Console.WriteLine("Not enough quantity available.");
                return;
            }

            item.Quantity -= quantity;

            var cartItem = cart.FirstOrDefault(i => i.Id == id);
            if (cartItem == null)
            {
                cart.Add(new InventoryItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Quantity = quantity
                });
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            Console.WriteLine("Item added to cart.");
        }

        private void RemoveFromCart(List<InventoryItem> cart)
        {
            Console.Write("Enter item ID to remove from cart: ");
            var id = int.Parse(Console.ReadLine() ?? "0");

            var cartItem = cart.FirstOrDefault(i => i.Id == id);
            if (cartItem == null)
            {
                Console.WriteLine("Item not found in cart.");
                return;
            }

            Console.Write("Enter quantity to remove from cart: ");
            var quantity = int.Parse(Console.ReadLine() ?? "0");

            if (quantity > cartItem.Quantity)
            {
                Console.WriteLine("Not enough quantity in cart.");
                return;
            }

            cartItem.Quantity -= quantity;

            if (cartItem.Quantity == 0)
            {
                cart.Remove(cartItem);
            }

            var inventoryItem = inventorySvc.Items?.FirstOrDefault(i => i.Id == id);
            if (inventoryItem != null)
            {
                inventoryItem.Quantity += quantity;
            }

            Console.WriteLine("Item removed from cart.");
        }

        private void Checkout(List<InventoryItem> cart)
        {
            double? subtotal = 0;
            foreach (var item in cart)
            {
                subtotal += item.Price * item.Quantity;
            }

            double? tax = subtotal * 0.07;
            double? total = subtotal + tax;

            Console.WriteLine("Receipt:");
            Console.WriteLine();
            Console.WriteLine("         _      _____ _____ \r\n     /\\   | |    |  __ \\_   _|\r\n    " +
                "/  \\  | |    | |  | || |  \r\n   / /\\ \\ | |    | |  | || |  \r\n  " +
                "/ ____ \\| |____| |__| || |_ \r\n /_/    \\_\\______|_____/_____|");
            Console.WriteLine();
            foreach (var item in cart)
            {
                Console.WriteLine($"{item.Name} - {item.Quantity} * ${item.Price} = ${item.Price * item.Quantity}");
            }
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Subtotal: ${subtotal:F2}");
            Console.WriteLine($"Tax (7%): ${tax:F2}");
            Console.WriteLine("_______________________________");
            Console.WriteLine($"Total: ${total:F2}");
            Console.WriteLine();

            cart.Clear();
        }

    }
}

