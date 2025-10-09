namespace InventoryApp.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Product()
        {
            
        }
        public Product(int id, string name, int quantity, decimal price)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Price = price;
        }
    }
}
