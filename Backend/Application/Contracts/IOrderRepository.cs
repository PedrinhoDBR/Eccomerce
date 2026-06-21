using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Contracts;

/// <summary>
/// Persistence contract for orders.
/// </summary>
public interface IOrderRepository
{
    void Add(Order order);
    IReadOnlyCollection<Order> GetAll();
}
