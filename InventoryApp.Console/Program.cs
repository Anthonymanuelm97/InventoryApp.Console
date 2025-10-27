using InventoryApp.BL.Services;
using InventoryApp.Repository.Helper;
using InventoryApp.Repository.Repositories;
using InventoryApp.Entities.Models;
using System.Transactions;
internal class Program
{
    static void Main(string[] args)
    {
        // Step 1: Setup dependencies
        var dbHelper = new DbConnectionHelper(@"Server=DESKTOP-NQ6DHBE;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;");
        var repository = new ProductRepository(dbHelper);
        var service = new ProductService(repository);

        int option = 0;

        while (option != 6)
        {
            Console.WriteLine("\n*** INVENTORY SYSTEM ***");
            Console.WriteLine("1 - Add Product");
            Console.WriteLine("2 - Show All Products");
            Console.WriteLine("3 - Show Product by ID");
            Console.WriteLine("4 - Update Product");
            Console.WriteLine("5 - Remove Product");
            Console.WriteLine("6 - Exit");
            Console.WriteLine("Choose an option: ");

            var input = Console.ReadLine();

            if (!int.TryParse(input, out option))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (option)
            {
                case 1:
                    {
                        Console.WriteLine("\n--- Add Product ---");
                        Console.WriteLine("Enter Product name: ");
                        var name = Console.ReadLine();

                        // Replace the price input section in case 1 with a while loop for validation
                        Console.WriteLine("Enter Product price: ");
                        decimal price;
                        if (!decimal.TryParse(Console.ReadLine(), out price))
                        {
                            Console.WriteLine("Invalid price. Please enter a valid decimal number.");
                            break;                        
                        }
                        
                        Console.WriteLine("Enter Product quantity: ");
                        int quantity;
                        if (!int.TryParse(Console.ReadLine(), out quantity))
                        {
                            Console.WriteLine("Invalid quantity. Please enter a valid integer.");
                            break;
                        }

                        // Create a new product instance
                        Product newProduct = new Product
                        {
                            Name = name,
                            Quantity = quantity,
                            Price = price
                            
                        };

                        //Add the product using the service

                        var added = service.AddProduct(newProduct);
                        Console.WriteLine(added
                            ? "Product added Successfully."
                            : "Failed to add product.");

                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("\n--- List of Products ---");

                        var products = service.GetAllProducts();

                        if (products == null || !products.Any())
                        {
                            Console.WriteLine("No products found.");
                            break;
                        }
                        foreach (var product in products)
                        {
                            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}");
                        }

                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("\n--- Show Product by ID ---");
                        Console.WriteLine("Enter Product ID: ");

                        //1) Validate input 

                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            Console.WriteLine("Invalid ID. Please enter a valid number.");
                            break;
                        }

                        //2) Get the product by ID using the service
                        var product = service.GetProductById(id);

                        //3) Display the product details or a not found message
                        if (product == null)
                        {
                            Console.WriteLine($"Product with ID {id} not found.");
                        }
                        else
                        {
                            Console.WriteLine("\nProduct Details:");
                            Console.WriteLine($"ID: {product.Id}");
                            Console.WriteLine($"Name: {product.Name}");
                            Console.WriteLine($"Price: {product.Price}");
                            Console.WriteLine($"Quantity: {product.Quantity}");
                        }
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("\n--- Update Product ---");

                        Console.WriteLine("Enter product ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            Console.WriteLine("Invalid ID. Please enter a valid number.");
                            break;
                        }

                        var productToUpdate = service.GetProductById(updateId);
                        if (productToUpdate == null)
                        {
                            Console.WriteLine($"Product with ID {updateId} not found");
                            break;
                        }
                        Console.WriteLine($"Updating product: {productToUpdate.Name}");

                        //Name
                        Console.WriteLine($"Enter new name (leave blank to keep '{productToUpdate.Name}'): ");
                        string? newName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName))
                            productToUpdate.Name = newName;

                        //Price 
                        Console.WriteLine($"Enter new price (leave blank to keep '{productToUpdate.Price}'): ");
                        string? newPriceInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newPriceInput))
                        {
                            if (decimal.TryParse(newPriceInput, out decimal newPrice))
                            {
                                productToUpdate.Price = newPrice;
                            }
                            else
                            {
                                Console.WriteLine("Invalid price. Keeping old value.");
                            }
                        }

                        //Quantity
                        Console.WriteLine($"Enter new quantity (leave blank to keep '{productToUpdate.Quantity}'): ");
                        string? newQuantityInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newQuantityInput))
                        {
                            if (int.TryParse(newQuantityInput, out int newQuantity))
                            {
                                productToUpdate.Quantity = newQuantity;
                            }
                            else
                            {
                                Console.WriteLine("Invalid quantity. Keeping old value.");
                            }
                        }
                        //Save Changes
                        var updated = service.UpdateProduct(productToUpdate);
                        Console.WriteLine(updated
                            ? "Product updated successfully."
                            : "Failed to update product. ");
                        break;
                    }
                case 5:
                    Console.WriteLine("\n--- Remove Product ---");

                    Console.WriteLine("Enter Product ID to remove: ");
                    if (!int.TryParse(Console.ReadLine(), out int removeId))
                    {
                        Console.WriteLine("Invalid ID. Please enter a valid number.");
                        break;
                    }

                    var productToRemove = service.GetProductById(removeId);
                    if (productToRemove == null)
                    {
                        Console.WriteLine($"Product with ID {removeId} not found.");
                        break;
                    }

                    Console.WriteLine($"Are you sure you want to remove '{productToRemove.Name}'? (y/n): ");
                    var confirm = Console.ReadLine();
                    if (confirm?.ToLower() != "y")
                    {
                        Console.WriteLine("Operation canceled.");
                        break;
                    }

                    var removed = service.RemoveProduct(removeId);
                    Console.WriteLine(removed
                        ? "Product removed successfully."
                        : "Failed to remove product.");
                    break;

                case 6:
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;

            }
        }
    }
}

