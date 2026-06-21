namespace Ecommerce.Contracts.Responses;

/// <summary>
/// Order data returned to clients.
/// </summary>
public sealed record OrderResponse(
    Guid Id,
    string CustomerName,
    string CustomerEmail,
    string Address,
    string Phone,
    DateTime CreatedAt,
    string Status,
    decimal Total,
    IReadOnlyCollection<OrderItemResponse> Items);
