using Ecommerce.Application.Contracts;
using Ecommerce.Contracts.Responses;
using Ecommerce.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrdersController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet]
    [ServiceFilter(typeof(AdminAuthorizationFilter))]
    public ActionResult<IReadOnlyCollection<OrderResponse>> GetAll()
    {
        var orders = _orderRepository
            .GetAll()
            .OrderByDescending(order => order.CreatedAt)
            .Select(OrderMapping.ToResponse)
            .ToList()
            .AsReadOnly();

        return Ok(orders);
    }
}
