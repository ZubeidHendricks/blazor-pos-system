using BlazorPOS.Shared.Models;

namespace BlazorPOS.Server.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(List<CartItem> items, string paymentMethod);
        Task<bool> ProcessPaymentAsync(int orderId, decimal amount, string paymentMethod);
        Task<Order?> GetOrderAsync(int orderId);
    }
}