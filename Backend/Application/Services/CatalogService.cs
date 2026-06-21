using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services;

/// <summary>
/// Use case for querying the product catalog.
/// </summary>
public sealed class CatalogService
{
    private readonly IProductRepository _productRepository;

    public CatalogService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IReadOnlyCollection<Product> ListAvailableProducts()
    {
        return _productRepository
            .GetAll()
            .Where(product => product.StockQuantity > 0)
            .OrderBy(product => product.Name)
            .ToList()
            .AsReadOnly();
    }
}
