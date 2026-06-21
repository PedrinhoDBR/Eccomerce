namespace Ecommerce.Contracts.Responses;

/// <summary>
/// Order data returned to clients.
/// </summary>
public sealed record OrderResponse(
    Guid Id,
    string CustomerName,
    string CustomerEmail,
    DateTime CreatedAt,
    string Status,
    decimal Total,
    IReadOnlyCollection<OrderItemResponse> Items);
