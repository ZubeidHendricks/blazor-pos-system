using BlazorPOS.Client.Models;

namespace BlazorPOS.Client.Services
{
    public class CartService
    {
        public List<CartItem> Items { get; private set; } = new List<CartItem>();
        
        public decimal Total => Items.Sum(item => item.Subtotal);
        
        public event Action OnChange;
        
        public void AddItem(Product product)
        {
            var existingItem = Items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existingItem != null)
            {
                UpdateQuantity(existingItem, existingItem.Quantity + 1);
            }
            else
            {
                Items.Add(new CartItem { Product = product, Quantity = 1 });
                NotifyStateChanged();
            }
        }
        
        public void UpdateQuantity(CartItem item, int quantity)
        {
            if (quantity <= 0)
                RemoveItem(item);
            else
            {
                item.Quantity = quantity;
                NotifyStateChanged();
            }
        }
        
        public void RemoveItem(CartItem item)
        {
            Items.Remove(item);
            NotifyStateChanged();
        }
        
        public void Clear()
        {
            Items.Clear();
            NotifyStateChanged();
        }
        
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}