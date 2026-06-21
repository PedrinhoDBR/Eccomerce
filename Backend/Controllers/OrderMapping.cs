using Ecommerce.Contracts.Responses;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Controllers;

internal static class OrderMapping
{
    public static OrderResponse ToResponse(Order order)
    {
        var items = order.Items
            .Select(item => new OrderItemResponse(
                item.ProductId,
                item.ProductName,
                item.UnitPrice.Amount,
                item.Quantity,
                item.Subtotal.Amount))
            .ToList()
            .AsReadOnly();

        return new OrderResponse(
            order.Id,
            order.Customer.FullName,
            order.Customer.Email,
            order.Customer.Address,
            order.Customer.Phone,
            order.CreatedAt,
            order.Status.ToString(),
            order.Total.Amount,
            items);
    }
}
