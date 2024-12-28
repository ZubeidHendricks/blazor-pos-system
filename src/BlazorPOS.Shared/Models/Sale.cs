namespace BlazorPOS.Shared.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public SaleStatus Status { get; set; }
        public List<SaleItem> Items { get; set; }
    }

    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public enum SaleStatus
    {
        Pending,
        Completed,
        Cancelled
    }
}