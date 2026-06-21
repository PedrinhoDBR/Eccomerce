using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities;

/// <summary>
/// Order confirmed after the shopping cart checkout.
/// </summary>
public sealed class Order
{
    private readonly List<OrderItem> _items;

    public Order(Customer customer, IEnumerable<OrderItem> items)
    {
        _items = items.ToList();

        if (!_items.Any())
        {
            throw new InvalidOperationException("The order must contain at least one item.");
        }

        Id = Guid.NewGuid();
        Customer = customer;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Created;
    }

    public Guid Id { get; }
    public Customer Customer { get; }
    public DateTime CreatedAt { get; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money Total => _items.Aggregate(Money.Zero, (total, item) => total + item.Subtotal);

    public void MarkAsPaid() => Status = OrderStatus.Paid;
}
