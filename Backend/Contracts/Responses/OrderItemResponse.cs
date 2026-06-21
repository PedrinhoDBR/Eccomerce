namespace Ecommerce.Contracts.Responses;

/// <summary>
/// Order line returned to clients.
/// </summary>
public sealed record OrderItemResponse(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal Subtotal);
