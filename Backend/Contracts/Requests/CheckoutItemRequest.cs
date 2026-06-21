namespace Ecommerce.Contracts.Requests;

/// <summary>
/// Product and quantity selected for checkout.
/// </summary>
public sealed record CheckoutItemRequest(Guid ProductId, int Quantity);
