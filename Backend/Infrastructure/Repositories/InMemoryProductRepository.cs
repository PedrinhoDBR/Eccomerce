using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Infrastructure.Repositories;

/// <summary>
/// In-memory repository used for demos and study.
/// In a real project, this class could be replaced by Entity Framework.
/// </summary>
public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new()
    {
        new Product(Guid.Parse("1c4f8f15-6e0f-4c1c-9c6e-0ff2d661f101"), "Notebook Pro 14", "Notebook for productivity and software development.", new Money(5299.90m), 8),
        new Product(Guid.Parse("2b71a205-8d8d-4d3e-9c55-8c8df8d70f02"), "Ergonomic Mouse", "Wireless mouse with high precision.", new Money(189.90m), 25),
        new Product(Guid.Parse("3d7e8b19-74a4-4ed5-8b91-c8e92cfd2103"), "Mechanical Keyboard", "Keyboard with tactile switches.", new Money(349.90m), 15),
        new Product(Guid.Parse("4ea6f8c4-96b5-4b8a-8bc3-70cf0c937104"), "27-inch Monitor", "QHD monitor with high color accuracy.", new Money(1599.00m), 6)
    };

    public IReadOnlyCollection<Product> GetAll() => _products.AsReadOnly();

    public Product? GetById(Guid id) => _products.FirstOrDefault(product => product.Id == id);

    public void Update(Product product)
    {
        var index = _products.FindIndex(current => current.Id == product.Id);

        if (index < 0)
        {
            throw new InvalidOperationException("Product not found for update.");
        }

        _products[index] = product;
    }
}
