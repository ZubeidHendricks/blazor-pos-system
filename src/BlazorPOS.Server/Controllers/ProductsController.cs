using Microsoft.AspNetCore.Mvc;
using BlazorPOS.Server.Services;
using BlazorPOS.Shared.Models;

namespace BlazorPOS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<Product>> GetProductByBarcode(string barcode)
        {
            var product = await _productRepository.GetByBarcodeAsync(barcode);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int newStock)
        {
            if (newStock < 0) return BadRequest("Stock cannot be negative");

            var success = await _productRepository.UpdateStockAsync(id, newStock);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}