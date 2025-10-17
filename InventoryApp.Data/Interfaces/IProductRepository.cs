using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryApp.Entities.Models;

namespace InventoryApp.Repository.Interfaces
{
    public interface IProductRepository
    {
        bool AddProduct(Product product);
        List<Product> GetAllProducts();
        Product? GetProductById(int id);
        bool UpdateProduct(Product product);
        bool RemoveProduct(int id);

        Product? GetLastInsertedProduct();

    }
}
