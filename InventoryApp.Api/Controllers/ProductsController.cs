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

            var createdProduct = _productService.AddProduct(product);
            if(createdProduct == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the product");
           
            return CreatedAtAction(nameof(GetProductById), new {id = createdProduct.Id }, createdProduct);
        }
    }
}
