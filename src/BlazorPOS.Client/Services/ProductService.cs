public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Shared.Models.Product> GetProductByIdAsync(int id)
    {
        // Replace with actual implementation
        var product = await _httpClient.GetFromJsonAsync<Shared.Models.Product>($"api/products/{id}");
        return product ?? throw new InvalidOperationException($"Product with id {id} not found");
    }

    public async Task<Shared.Models.Product> GetProductByBarcodeAsync(string barcode)
    {
        // Replace with actual implementation
        var product = await _httpClient.GetFromJsonAsync<Shared.Models.Product>($"api/products/barcode/{barcode}");
        return product ?? throw new InvalidOperationException($"Product with barcode {barcode} not found");
    }
}

public interface IProductService
{
    Task<Shared.Models.Product> GetProductByIdAsync(int id);
    Task<Shared.Models.Product> GetProductByBarcodeAsync(string barcode);
}