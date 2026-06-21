using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Infrastructure.Repositories;

/// <summary>
/// In-memory repository for storing orders created during execution.
/// </summary>
public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    public void Add(Order order) => _orders.Add(order);

    public IReadOnlyCollection<Order> GetAll() => _orders.AsReadOnly();
}
