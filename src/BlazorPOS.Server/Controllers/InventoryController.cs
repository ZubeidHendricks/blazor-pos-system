using Microsoft.AspNetCore.Authorization;

namespace BlazorPOS.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "ViewInventory")]
        public IActionResult GetInventory()
        {
            // Implement inventory retrieval logic
            return Ok(new { message = "Inventory accessed successfully" });
        }

        [HttpPost]
        [Authorize(Policy = "EditProduct")]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            // Implement product update logic
            return Ok(new { message = "Product updated successfully" });
        }
    }
}