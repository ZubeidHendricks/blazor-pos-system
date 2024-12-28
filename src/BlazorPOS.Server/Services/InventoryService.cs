using Microsoft.EntityFrameworkCore;

namespace BlazorPOS.Server.Services
{
    public interface IInventoryService
    {
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<InventoryTransaction> AdjustInventoryAsync(int productId, int quantity, string userId, string notes);
        Task<List<InventoryTransaction>> GetProductTransactionsAsync(int productId);
        Task<List<Product>> GetLowStockProductsAsync(int threshold = 10);
    }

    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<InventoryTransaction> AdjustInventoryAsync(int productId, int quantity, string userId, string notes)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                    throw new NotFoundException($"Product with ID {productId} not found");

                product.StockQuantity += quantity;

                var inventoryTransaction = new InventoryTransaction
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Type = quantity > 0 ? TransactionType.StockIn : TransactionType.StockOut,
                    UserId = userId,
                    Timestamp = DateTime.UtcNow,
                    Notes = notes
                };

                _context.Products.Update(product);
                _context.InventoryTransactions.Add(inventoryTransaction);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return inventoryTransaction;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<InventoryTransaction>> GetProductTransactionsAsync(int productId)
        {
            return await _context.InventoryTransactions
                .Where(t => t.ProductId == productId)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }

        public async Task<List<Product>> GetLowStockProductsAsync(int threshold = 10)
        {
            return await _context.Products
                .Where(p => p.StockQuantity <= threshold)
                .ToListAsync();
        }
    }
}