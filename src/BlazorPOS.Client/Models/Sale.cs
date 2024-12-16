namespace BlazorPOS.Client.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
    }
}