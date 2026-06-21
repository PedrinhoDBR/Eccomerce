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
        : this(customer, items, Guid.NewGuid(), DateTime.UtcNow, OrderStatus.Created)
    {
    }

    public Order(Customer customer, IEnumerable<OrderItem> items, Guid id, DateTime createdAt, OrderStatus status)
    {
        _items = items.ToList();

        if (!_items.Any())
        {
            throw new InvalidOperationException("The order must contain at least one item.");
        }

        Id = id;
        Customer = customer;
        CreatedAt = createdAt;
        Status = status;
    }

    public Guid Id { get; }
    public Customer Customer { get; }
    public DateTime CreatedAt { get; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money Total => _items.Aggregate(Money.Zero, (total, item) => total + item.Subtotal);

    public void MarkAsPaid() => Status = OrderStatus.Paid;
}
