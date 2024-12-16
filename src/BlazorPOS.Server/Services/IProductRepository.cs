using BlazorPOS.Shared.Models;

namespace BlazorPOS.Server.Services
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByBarcodeAsync(string barcode);
        Task<bool> UpdateStockAsync(int productId, int newStock);
    }
}