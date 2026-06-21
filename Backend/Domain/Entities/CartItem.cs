using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities;

/// <summary>
/// Item added to the cart with product, quantity, and subtotal.
/// </summary>
public sealed class CartItem
{
    public CartItem(Product product, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        Product = product;
        Quantity = quantity;
    }

    public Product Product { get; }
    public int Quantity { get; private set; }
    public Money Subtotal => Product.Price * Quantity;

    public void AddQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        Quantity += quantity;
    }
}
