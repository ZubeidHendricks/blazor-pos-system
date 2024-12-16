namespace BlazorPOS.Shared.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Barcode { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public required string Category { get; set; }
    }
}