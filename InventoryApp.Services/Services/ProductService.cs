using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryApp.Repository.Interfaces;
using InventoryApp.Entities.Models;

namespace InventoryApp.BL.Services
{
    public class ProductService
    {
        //Dependency of the data layer  through the repository interface
        private readonly IProductRepository _productRepository;

        //Constructor that receives the implementation of the repository interface(Dependency Injection)
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public bool AddProduct(Product product)
        {

            return _productRepository.AddProduct(product);

        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }
        public Product? GetProductById(int id)
        {

            return _productRepository.GetProductById(id);
        }

        public bool UpdateProduct(Product product)
        {
            return _productRepository.UpdateProduct(product);
        }

        public bool RemoveProduct(int id)
        {
            return _productRepository.RemoveProduct(id);
        }
    }
}
