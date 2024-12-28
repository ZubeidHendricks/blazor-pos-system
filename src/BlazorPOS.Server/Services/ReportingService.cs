using Microsoft.EntityFrameworkCore;

namespace BlazorPOS.Server.Services
{
    public interface IReportingService
    {
        Task<SalesSummaryReport> GetSalesSummaryAsync(DateTime startDate, DateTime endDate);
        Task<InventoryReport> GetInventoryReportAsync();
    }

    public class ReportingService : IReportingService
    {
        private readonly ApplicationDbContext _context;

        public ReportingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SalesSummaryReport> GetSalesSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .Include(s => s.Items)
                .ToListAsync();

            return new SalesSummaryReport
            {
                TotalRevenue = sales.Sum(s => s.TotalAmount),
                TotalSales = sales.Count,
                AverageOrderValue = sales.Any() ? sales.Average(s => s.TotalAmount) : 0,
                TopProducts = sales
                    .SelectMany(s => s.Items)
                    .GroupBy(i => i.Product.Name)
                    .Select(g => new TopSellingProduct
                    {
                        ProductName = g.Key,
                        QuantitySold = g.Sum(i => i.Quantity),
                        TotalRevenue = g.Sum(i => i.Quantity * i.UnitPrice)
                    })
                    .OrderByDescending(p => p.QuantitySold)
                    .Take(5)
                    .ToList()
            };
        }

        public async Task<InventoryReport> GetInventoryReportAsync()
        {
            var products = await _context.Products.ToListAsync();

            return new InventoryReport
            {
                TotalUniqueProducts = products.Count,
                TotalInventoryValue = products.Sum(p => p.StockQuantity * p.Price),
                LowStockProducts = products
                    .Where(p => p.StockQuantity <= 10)
                    .Select(p => new LowStockProduct
                    {
                        Name = p.Name,
                        CurrentStock = p.StockQuantity,
                        UnitPrice = p.Price
                    })
                    .ToList()
            };
        }
    }
}