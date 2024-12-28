using Microsoft.EntityFrameworkCore;

namespace BlazorPOS.Server.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummary> GetDashboardSummaryAsync();
    }

    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummary> GetDashboardSummaryAsync()
        {
            var today = DateTime.UtcNow.Date;
            var monthStart = new DateTime(today.Year, today.Month, 1);

            var todaySales = await _context.Sales
                .Where(s => s.SaleDate >= today)
                .ToListAsync();

            var monthSales = await _context.Sales
                .Where(s => s.SaleDate >= monthStart)
                .ToListAsync();

            var lowStockProducts = await _context.Products
                .Where(p => p.StockQuantity <= 10)
                .Select(p => new LowStockAlert
                {
                    ProductName = p.Name,
                    CurrentStock = p.StockQuantity
                })
                .ToListAsync();

            var recentSales = await _context.Sales
                .OrderByDescending(s => s.SaleDate)
                .Take(5)
                .Select(s => new RecentSale
                {
                    SaleId = s.Id,
                    UserName = s.UserId,
                    TotalAmount = s.TotalAmount,
                    SaleDate = s.SaleDate
                })
                .ToListAsync();

            return new DashboardSummary
            {
                TodayRevenue = todaySales.Sum(s => s.TotalAmount),
                TodaySalesCount = todaySales.Count,
                MonthToDateRevenue = monthSales.Sum(s => s.TotalAmount),
                RecentSales = recentSales,
                LowStockAlerts = lowStockProducts
            };
        }
    }
}