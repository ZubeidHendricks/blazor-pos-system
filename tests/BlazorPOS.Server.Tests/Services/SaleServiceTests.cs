using Moq;
using Microsoft.EntityFrameworkCore;

namespace BlazorPOS.Server.Tests.Services
{
    public class SaleServiceTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly SaleService _saleService;

        public SaleServiceTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _saleService = new SaleService(_mockContext.Object);
        }

        [Fact]
        public async Task CreateSaleAsync_SufficientStock_ShouldCreateSale()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", StockQuantity = 10, Price = 9.99m };
            var sale = new Sale 
            { 
                Items = new List<SaleItem> 
                { 
                    new SaleItem { ProductId = 1, Quantity = 5, UnitPrice = 9.99m } 
                } 
            };

            var mockProductSet = new Mock<DbSet<Product>>();
            var mockSaleSet = new Mock<DbSet<Sale>>();

            mockProductSet.Setup(m => m.FindAsync(It.IsAny<int>())).ReturnsAsync(product);
            _mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);
            _mockContext.Setup(c => c.Sales).Returns(mockSaleSet.Object);

            // Act
            var result = await _saleService.CreateSaleAsync(sale);

            // Assert
            mockSaleSet.Verify(m => m.Add(It.IsAny<Sale>()), Times.Once);
            Assert.Equal(5, product.StockQuantity);
        }

        [Fact]
        public async Task CreateSaleAsync_InsufficientStock_ShouldThrowException()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", StockQuantity = 3, Price = 9.99m };
            var sale = new Sale 
            { 
                Items = new List<SaleItem> 
                { 
                    new SaleItem { ProductId = 1, Quantity = 5, UnitPrice = 9.99m } 
                } 
            };

            var mockProductSet = new Mock<DbSet<Product>>();
            mockProductSet.Setup(m => m.FindAsync(It.IsAny<int>())).ReturnsAsync(product);
            _mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _saleService.CreateSaleAsync(sale));
        }
    }
}