using InventoryApp.Repository.Helper;
using InventoryApp.Repository.Interfaces;
using InventoryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using InventoryApp.Entities.Models;

namespace InventoryApp.Repository.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbConnectionHelper _dbHelper;

        public ProductRepository(DbConnectionHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool AddProduct(Product product)
        {
            //Query to insert a new product into the database avoiding SQL injection
            const string query = "INSERT INTO Products (Name, Price, Quantity) VALUES (@Name, @Price, @Quantity)";

            try
            {
                using var connection = _dbHelper.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = query; // Set the command text to the query

                //Adding parameters

                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery(); // Execute the query

                return rowsAffected > 0; // Return true if at least one row was affected
            }


            catch (Exception ex)
            {
                return false;
            }


        }

        public List<Product> GetAllProducts()
        {
            const string query = "SELECT Id, Name, Price, Quantity FROM Products";
            var products = new List<Product>();

            try
            {
                using var connection = _dbHelper.CreateConnection();
                using var command = connection.CreateCommand();
                command.CommandText = query;

                connection.Open();
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var product = new Product
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Quantity = reader.GetInt32(3)
                    };
                    products.Add(product);
                }

                return products;


            }
            catch (Exception)
            {

                return new List<Product>(); // Return an empty list in case of an error
            }
        }

        public Product? GetProductById(int id)
        {
            const string query = " SELECT Id, Name, Price, Quantity FROM Products WHERE Id = @Id";

            try
            {
                using var connection = _dbHelper.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = query; // Set the command text to the query
                //Add secure parameters to avoid SQL injection
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();


                using var reader = command.ExecuteReader(); // Execute the query

                if (reader.Read()) //If any register is found
                {
                    return new Product
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Quantity = reader.GetInt32(3)
                    };
                }

                return null; // Return null if no product is found
            }
            catch (Exception)
            {
                return null; // Return null in case of an error

            }
        }

        public bool UpdateProduct(Product product)
        {
            //Check if Product exists
            var existingProduct = GetProductById(product.Id);
            if (existingProduct == null)
                return false; //Cannot update a non-existing product

            const string query = "UPDATE Products SET Name = @Name, Price = @Price, Quantity = @Quantity WHERE Id = @Id";

            try
            {
                using var connection = _dbHelper.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = query; // Set the command text to the query

                //Adding parameters
                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0; // Return true if at least one row was affected                
            }
            catch (Exception)
            {

                return false; // Error occurred, return false
            }

        }
        public bool RemoveProduct(int id)
        {
            var existingProduct = GetProductById(id);
            if (existingProduct == null)
                return false; // Cannot remove a non-existing product

            const string query = "DELETE FROM Products WHERE Id = @Id";

            try
            {
                using var connection = _dbHelper.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = query; // Set the command text to the query
                //Adding parameters
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery(); // Execute the query

                return rowsAffected > 0; // Return true if at least one row was affected
            }
            catch (Exception)
            {
                return false; // Error occurred, return false
            }
        }
    }
}
