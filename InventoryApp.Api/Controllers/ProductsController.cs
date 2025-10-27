using InventoryApp.BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        // Constructor that receives the implementation of the service class (Dependency Injection)
        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _productService.GetAllProducts();

            if (products == null || products.Count == 0)
                return NotFound("Products not found");

            return Ok(products);
        }

        [HttpGetAttribute("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
                return NotFound($"Product with id {id} not found");

            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] Entities.Models.Product product)
        {

            try
            {
                if (product == null)
                    return BadRequest("Product data cannot be null");

                var isAdded = _productService.AddProduct(product);

                //Managing potential errors during the insertion process
                if (!isAdded)
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the product");

                //Managing where may occur asinchrony or transaction issues, prevent returning an incomplete response and improve flow tracking
                var createdProduct = _productService.GetLastInsertedProduct();
                if (createdProduct == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve the created product.");

                // Complying with RESTful conventions by returning a 201 Created response with the location of the new resource
                return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddProduct: {ex.Message}");
                throw;
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Entities.Models.Product product)
        {
            if (product == null)
                return BadRequest("Product data cannot be null");

            if (id != product.Id)
                return BadRequest("Product ID mismatch");

            var existingProduct = _productService.GetProductById(id);
            if (existingProduct == null)
                return NotFound($"Product with id {id} not found");

            _productService.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("{id}")]

        public IActionResult RemoveProduct(int id)

        {
            //Check if the product exist before attempting to delete it
            var existingProduct = _productService.GetProductById(id);
            if (existingProduct == null)
                return NotFound($"Product with id {id} not found");

            //Proceed to delete the product
            var isRemoved = _productService.RemoveProduct(id);

            //Handle potential errors during the deletion process

            if (!isRemoved)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product. ");


            //Return NoContent to indicate successful deletion without returning any content as per RESTful standards.
            return NoContent();

        }
    }
}
