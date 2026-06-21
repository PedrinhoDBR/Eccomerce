namespace Ecommerce.Contracts.Responses;

/// <summary>
/// Product data returned to clients.
/// </summary>
public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity);
