using Microsoft.EntityFrameworkCore;

namespace BlazorPOS.Server.Services
{
    public interface ISaleService
    {
        Task<Sale> CreateSaleAsync(Sale sale);
        Task<Sale> GetSaleByIdAsync(int saleId);
        Task<List<Sale>> GetSalesByUserAsync(string userId);
        Task<Sale> UpdateSaleStatusAsync(int saleId, SaleStatus status);
    }

    public class SaleService : ISaleService
    {
        private readonly ApplicationDbContext _context;

        public SaleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            // Begin transaction to ensure inventory and sale are updated atomically
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update inventory
                foreach (var item in sale.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null || product.StockQuantity < item.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product {item.ProductId}");
                    }

                    product.StockQuantity -= item.Quantity;
                    _context.Products.Update(product);
                }

                sale.SaleDate = DateTime.UtcNow;
                sale.Status = SaleStatus.Completed;

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return sale;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Sale> GetSaleByIdAsync(int saleId)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId);
        }

        public async Task<List<Sale>> GetSalesByUserAsync(string userId)
        {
            return await _context.Sales
                .Where(s => s.UserId == userId)
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<Sale> UpdateSaleStatusAsync(int saleId, SaleStatus status)
        {
            var sale = await _context.Sales.FindAsync(saleId);
            if (sale == null)
            {
                throw new NotFoundException($"Sale with ID {saleId} not found");
            }

            sale.Status = status;
            await _context.SaveChangesAsync();
            return sale;
        }
    }
}