using Microsoft.AspNetCore.Mvc;
using BlazorPOS.Server.Services;
using BlazorPOS.Shared.Models;

namespace BlazorPOS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderRequest request)
        {
            var order = await _orderService.CreateOrderAsync(request.Items, request.PaymentMethod);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> ProcessPayment(int id, [FromBody] PaymentRequest request)
        {
            var success = await _orderService.ProcessPaymentAsync(id, request.Amount, request.PaymentMethod);
            if (!success) return NotFound();
            return NoContent();
        }
    }

    public class OrderRequest
    {
        public List<CartItem> Items { get; set; } = new();
        public string PaymentMethod { get; set; } = "";
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "";
    }
}