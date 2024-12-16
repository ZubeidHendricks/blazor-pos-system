using Microsoft.EntityFrameworkCore;
using BlazorPOS.Server.Data;
using BlazorPOS.Shared.Models;

namespace BlazorPOS.Server.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product?> GetByBarcodeAsync(string barcode)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Barcode == barcode);
        }

        public async Task<bool> UpdateStockAsync(int productId, int newStock)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            product.Stock = newStock;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}