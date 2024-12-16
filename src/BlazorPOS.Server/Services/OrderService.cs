using Microsoft.EntityFrameworkCore;
using BlazorPOS.Server.Data;
using BlazorPOS.Shared.Models;

namespace BlazorPOS.Server.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;

        public OrderService(ApplicationDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        public async Task<Order> CreateOrderAsync(List<CartItem> items, string paymentMethod)
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                PaymentMethod = paymentMethod,
                Items = items.Select(i => new OrderItem
                {
                    ProductId = i.Product.Id,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.Price
                }).ToList(),
                Total = items.Sum(i => i.Product.Price * i.Quantity)
            };

            _context.Orders.Add(order);
            
            // Update stock levels
            foreach (var item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.Product.Id);
                if (product != null)
                {
                    await _productRepository.UpdateStockAsync(product.Id, product.Stock - item.Quantity);
                }
            }

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> ProcessPaymentAsync(int orderId, decimal amount, string paymentMethod)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.PaymentStatus = "Paid";
            order.PaymentDate = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order?> GetOrderAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}