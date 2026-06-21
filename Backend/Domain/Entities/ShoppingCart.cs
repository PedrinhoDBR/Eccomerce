using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities;

/// <summary>
/// Shopping cart responsible for grouping items before checkout.
/// </summary>
public sealed class ShoppingCart
{
    private readonly List<CartItem> _items = new();

    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    public Money Total => _items.Aggregate(Money.Zero, (total, item) => total + item.Subtotal);
    public bool IsEmpty => !_items.Any();

    public void AddProduct(Product product, int quantity)
    {
        var item = _items.FirstOrDefault(cartItem => cartItem.Product.Id == product.Id);

        if (item is null)
        {
            _items.Add(new CartItem(product, quantity));
            return;
        }

        item.AddQuantity(quantity);
    }

    public void Clear() => _items.Clear();
}
