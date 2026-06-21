namespace Ecommerce.Contracts.Requests;

/// <summary>
/// Checkout payload sent by the Angular application.
/// </summary>
public sealed record CheckoutRequest(
    string CustomerName,
    string CustomerEmail,
    string Address,
    string Phone,
    IReadOnlyCollection<CheckoutItemRequest> Items);
