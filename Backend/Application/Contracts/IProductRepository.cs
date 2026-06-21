using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Contracts;

/// <summary>
/// Persistence contract for querying and updating products.
/// </summary>
public interface IProductRepository
{
    IReadOnlyCollection<Product> GetAll();
    Product? GetById(Guid id);
    void Add(Product product);
    void Update(Product product);
    void Delete(Guid id);
}
