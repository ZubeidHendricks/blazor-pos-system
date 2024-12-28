namespace BlazorPOS.Shared.Dtos
{
    public class SalesSummaryReport
    {
        public decimal TotalRevenue { get; set; }
        public int TotalSales { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<TopSellingProduct> TopProducts { get; set; }
    }

    public class TopSellingProduct
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class InventoryReport
    {
        public List<LowStockProduct> LowStockProducts { get; set; }
        public int TotalUniqueProducts { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }

    public class LowStockProduct
    {
        public string Name { get; set; }
        public int CurrentStock { get; set; }
        public decimal UnitPrice { get; set; }
    }
}