namespace BlazorPOS.Shared.Models
{
    public class InventoryTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public TransactionType Type { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Notes { get; set; }
    }

    public enum TransactionType
    {
        StockIn,
        StockOut,
        Adjustment,
        Sales,
        Returns
    }
}