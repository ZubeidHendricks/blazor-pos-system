namespace BlazorPOS.Shared.Dtos
{
    public class DashboardSummary
    {
        public decimal TodayRevenue { get; set; }
        public int TodaySalesCount { get; set; }
        public decimal MonthToDateRevenue { get; set; }
        public List<RecentSale> RecentSales { get; set; }
        public List<LowStockAlert> LowStockAlerts { get; set; }
    }

    public class RecentSale
    {
        public int SaleId { get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SaleDate { get; set; }
    }

    public class LowStockAlert
    {
        public string ProductName { get; set; }
        public int CurrentStock { get; set; }
    }
}