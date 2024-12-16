namespace BlazorPOS.Shared.Models
{
    public class CartItem
    {
        public required Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => Product.Price * Quantity;
    }
}