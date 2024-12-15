using Bunit;
using Xunit;
using BlazorPOS.Client.Components;
using BlazorPOS.Shared.Models;

namespace BlazorPOS.Client.Tests.Components
{
    public class ProductGridTests : TestContext
    {
        [Fact]
        public void ProductGrid_ShouldRender_WithTestData()
        {
            // Arrange
            var cut = RenderComponent<ProductGrid>();

            // Act
            var productCards = cut.FindAll(".product-card");

            // Assert
            Assert.NotEmpty(productCards);
        }

        [Fact]
        public void ProductGrid_ShouldFilter_WhenSearching()
        {
            // Arrange
            var cut = RenderComponent<ProductGrid>();
            var searchInput = cut.Find("input[type=text]");

            // Act
            searchInput.Input("Coffee");

            // Assert
            var filteredProducts = cut.FindAll(".product-card");
            Assert.Single(filteredProducts);
            Assert.Contains("Coffee", filteredProducts[0].TextContent);
        }

        [Fact]
        public void ProductGrid_ShouldTrigger_OnProductClicked()
        {
            // Arrange
            Product? selectedProduct = null;
            var cut = RenderComponent<ProductGrid>(parameters => parameters
                .Add(p => p.OnProductClicked, (Product p) => selectedProduct = p));

            // Act
            var firstProduct = cut.Find(".product-card");
            firstProduct.Click();

            // Assert
            Assert.NotNull(selectedProduct);
        }
    }
}
