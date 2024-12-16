using BlazorPOS.Shared.Models;

namespace BlazorPOS.Client.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> GetProductByBarcodeAsync(string barcode);
        Task<bool> UpdateStockAsync(int productId, int newStock);
    }
}