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

            if (products == null || !products.Any())
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
    }
}
//"Completed the AddProduct method with the GetLastInsertedProduct at the end to show the product added with new ID since it is autoincremented on Db"