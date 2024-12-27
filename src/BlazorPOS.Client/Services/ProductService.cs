public class ProductService : IProductService
{
    public async Task<Shared.Models.Product> GetProductByIdAsync(int id)
    {
        // Ensure this returns a non-null Product or throws an exception
        var product = await /* your existing logic */;
        return product ?? throw new InvalidOperationException($"Product with id {id} not found");
    }

    public async Task<Shared.Models.Product> GetProductByBarcodeAsync(string barcode)
    {
        // Ensure this returns a non-null Product or throws an exception
        var product = await /* your existing logic */;
        return product ?? throw new InvalidOperationException($"Product with barcode {barcode} not found");
    }
}

public interface IProductService
{
    Task<Shared.Models.Product> GetProductByIdAsync(int id);
    Task<Shared.Models.Product> GetProductByBarcodeAsync(string barcode);
}