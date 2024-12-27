using Microsoft.EntityFrameworkCore;
using BlazorPOS.Server.Data;

namespace BlazorPOS.Server.Services
{
    public class ReportingService : IReportingService
    {
        private readonly ApplicationDbContext _context;

        public ReportingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SalesReport> GenerateSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .Include(s => s.SaleItems)
                .ToListAsync();

            return new SalesReport 
            {
                TotalRevenue = sales.Sum(s => s.TotalAmount),
                TotalTransactions = sales.Count,
                TopSellingProducts = sales
                    .SelectMany(s => s.SaleItems)
                    .GroupBy(si => si.ProductId)
                    .Select(g => new TopProduct 
                    {
                        ProductId = g.Key,
                        ProductName = _context.Products.Find(g.Key).Name,
                        QuantitySold = g.Sum(si => si.Quantity)
                    })
                    .OrderByDescending(tp => tp.QuantitySold)
                    .Take(5)
                    .ToList()
            };
        }
    }
}