using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;
using LiteDB;

namespace Ecommerce.Infrastructure.Repositories;

public sealed class LiteDbOrderRepository : IOrderRepository
{
    private readonly ILiteCollection<OrderDocument> _orders;

    public LiteDbOrderRepository(LiteDatabase database)
    {
        _orders = database.GetCollection<OrderDocument>("orders");
        _orders.EnsureIndex(order => order.CreatedAt);
    }

    public void Add(Order order)
    {
        _orders.Insert(ToDocument(order));
    }

    public IReadOnlyCollection<Order> GetAll()
    {
        return _orders
            .FindAll()
            .Select(ToDomain)
            .ToList()
            .AsReadOnly();
    }

    private static Order ToDomain(OrderDocument document)
    {
        var customer = new Customer(
            document.CustomerName,
            document.CustomerEmail,
            document.Address,
            document.Phone);

        var items = document.Items.Select(item =>
            new OrderItem(item.ProductId, item.ProductName, new Money(item.UnitPrice), item.Quantity));

        var order = new Order(customer, items, document.Id, document.CreatedAt, document.Status);
        return order;
    }

    private static OrderDocument ToDocument(Order order)
    {
        return new OrderDocument
        {
            Id = order.Id,
            CustomerName = order.Customer.FullName,
            CustomerEmail = order.Customer.Email,
            Address = order.Customer.Address,
            Phone = order.Customer.Phone,
            CreatedAt = order.CreatedAt,
            Status = order.Status,
            Items = order.Items.Select(item => new OrderItemDocument
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice.Amount,
                Quantity = item.Quantity
            }).ToList()
        };
    }

    private sealed class OrderDocument
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDocument> Items { get; set; } = new();
    }

    private sealed class OrderItemDocument
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
