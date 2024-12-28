using Moq;
using Microsoft.EntityFrameworkCore;

namespace BlazorPOS.Server.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _productService = new ProductService(_mockContext.Object);
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProductToDatabase()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Barcode = "123456" };
            var mockSet = new Mock<DbSet<Product>>();
            _mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            // Act
            var result = await _productService.AddProductAsync(product);

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<Product>()), Times.Once);
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task GetProductByBarcodeAsync_ExistingBarcode_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Barcode = "123456" };
            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(new List<Product> { product }.AsQueryable().Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(new List<Product> { product }.AsQueryable().Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(new List<Product> { product }.AsQueryable().ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(new List<Product> { product }.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            // Act
            var result = await _productService.GetProductByBarcodeAsync("123456");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Product", result.Name);
        }
    }
}