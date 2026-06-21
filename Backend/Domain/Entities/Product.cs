using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities;

/// <summary>
/// Product available in the e-commerce catalog.
/// </summary>
public sealed class Product
{
    public Product(Guid id, string name, string description, Money price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The product name is required.", nameof(name));
        }

        if (stockQuantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stockQuantity), "Stock cannot be negative.");
        }

        Id = id;
        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        StockQuantity = stockQuantity;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public Money Price { get; }
    public int StockQuantity { get; private set; }

    public bool HasStock(int quantity) => quantity > 0 && StockQuantity >= quantity;

    public void DecreaseStock(int quantity)
    {
        if (!HasStock(quantity))
        {
            throw new InvalidOperationException($"Insufficient stock for product {Name}.");
        }

        StockQuantity -= quantity;
    }
}
