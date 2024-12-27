namespace BlazorPOS.Shared.Models
{
    public class InventoryTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
    }

    public enum TransactionType
    {
        Purchase,
        Sale,
        Adjustment
    }
}