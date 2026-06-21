using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services;

/// <summary>
/// Use case responsible for validating stock, creating an order, and decreasing stock.
/// </summary>
public sealed class CheckoutService
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public CheckoutService(IProductRepository productRepository, IOrderRepository orderRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public Order Checkout(Customer customer, ShoppingCart cart)
    {
        if (cart.IsEmpty)
        {
            throw new InvalidOperationException("Add products to the cart before checkout.");
        }

        foreach (var item in cart.Items)
        {
            var product = _productRepository.GetById(item.Product.Id);

            if (product is null || !product.HasStock(item.Quantity))
            {
                throw new InvalidOperationException($"Stock unavailable for {item.Product.Name}.");
            }
        }

        var orderItems = cart.Items.Select(item =>
            new OrderItem(item.Product.Id, item.Product.Name, item.Product.Price, item.Quantity));

        var order = new Order(customer, orderItems);

        foreach (var item in cart.Items)
        {
            item.Product.DecreaseStock(item.Quantity);
            _productRepository.Update(item.Product);
        }

        order.MarkAsPaid();
        _orderRepository.Add(order);
        cart.Clear();

        return order;
    }
}
