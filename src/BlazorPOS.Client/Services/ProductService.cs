using BlazorPOS.Client.Models;

namespace BlazorPOS.Client.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        
        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Product>>("api/products");
            }
            catch
            {
                // TODO: Add proper error handling
                return new List<Product>();
            }
        }
        
        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Product>($"api/products/{id}");
            }
            catch
            {
                // TODO: Add proper error handling
                return null;
            }
        }
        
        public async Task<Product> GetProductByBarcodeAsync(string barcode)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Product>($"api/products/barcode/{barcode}");
            }
            catch
            {
                // TODO: Add proper error handling
                return null;
            }
        }
        
        public async Task<bool> UpdateStockAsync(int productId, int newStock)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/products/{productId}/stock", newStock);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // TODO: Add proper error handling
                return false;
            }
        }
    }
}