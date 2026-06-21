using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities;

/// <summary>
/// Order line with a product snapshot from the purchase moment.
/// </summary>
public sealed class OrderItem
{
    public OrderItem(Guid productId, string productName, Money unitPrice, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid ProductId { get; }
    public string ProductName { get; }
    public Money UnitPrice { get; }
    public int Quantity { get; }
    public Money Subtotal => UnitPrice * Quantity;
}
