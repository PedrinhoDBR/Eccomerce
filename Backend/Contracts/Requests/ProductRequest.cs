namespace Ecommerce.Contracts.Requests;

public sealed record ProductRequest(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity);
