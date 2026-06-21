using Ecommerce.Application.Contracts;
using Ecommerce.Application.Services;
using Ecommerce.Contracts.Requests;
using Ecommerce.Contracts.Responses;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[Route("api/cart")]
public sealed class CheckoutController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly CheckoutService _checkoutService;

    public CheckoutController(IProductRepository productRepository, CheckoutService checkoutService)
    {
        _productRepository = productRepository;
        _checkoutService = checkoutService;
    }

    [HttpPost("checkout")]
    public ActionResult<OrderResponse> Checkout(CheckoutRequest request)
    {
        try
        {
            var customer = new Customer(request.CustomerName, request.CustomerEmail, request.Address, request.Phone);
            var cart = new ShoppingCart();

            foreach (var item in request.Items)
            {
                var product = _productRepository.GetById(item.ProductId);

                if (product is null)
                {
                    return BadRequest($"Product {item.ProductId} was not found.");
                }

                cart.AddProduct(product, item.Quantity);
            }

            var order = _checkoutService.Checkout(customer, cart);
            return CreatedAtAction(nameof(OrdersController.GetAll), "Orders", null, OrderMapping.ToResponse(order));
        }
        catch (Exception exception) when (exception is ArgumentException or ArgumentOutOfRangeException or InvalidOperationException)
        {
            return BadRequest(exception.Message);
        }
    }
}
