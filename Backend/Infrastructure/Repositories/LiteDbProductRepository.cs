using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;
using LiteDB;

namespace Ecommerce.Infrastructure.Repositories;

public sealed class LiteDbProductRepository : IProductRepository
{
    private readonly ILiteCollection<ProductDocument> _products;

    public LiteDbProductRepository(LiteDatabase database)
    {
        _products = database.GetCollection<ProductDocument>("products");
        _products.EnsureIndex(product => product.Name);
        SeedProducts();
    }

    public IReadOnlyCollection<Product> GetAll()
    {
        return _products
            .FindAll()
            .Select(ToDomain)
            .ToList()
            .AsReadOnly();
    }

    public Product? GetById(Guid id)
    {
        var document = _products.FindById(id);
        return document is null ? null : ToDomain(document);
    }

    public void Add(Product product)
    {
        _products.Insert(ToDocument(product));
    }

    public void Update(Product product)
    {
        if (!_products.Update(ToDocument(product)))
        {
            throw new InvalidOperationException("Product not found for update.");
        }
    }

    public void Delete(Guid id)
    {
        _products.Delete(id);
    }

    private void SeedProducts()
    {
        if (_products.Count() > 0)
        {
            return;
        }

        var seed = new[]
        {
            new Product(Guid.Parse("1c4f8f15-6e0f-4c1c-9c6e-0ff2d661f101"), "Notebook Pro 14", "Notebook for productivity and software development.", new Money(5299.90m), 8),
            new Product(Guid.Parse("2b71a205-8d8d-4d3e-9c55-8c8df8d70f02"), "Ergonomic Mouse", "Wireless mouse with high precision.", new Money(189.90m), 25),
            new Product(Guid.Parse("3d7e8b19-74a4-4ed5-8b91-c8e92cfd2103"), "Mechanical Keyboard", "Keyboard with tactile switches.", new Money(349.90m), 15),
            new Product(Guid.Parse("4ea6f8c4-96b5-4b8a-8bc3-70cf0c937104"), "27-inch Monitor", "QHD monitor with high color accuracy.", new Money(1599.00m), 6)
        };

        _products.InsertBulk(seed.Select(ToDocument));
    }

    private static Product ToDomain(ProductDocument document)
    {
        return new Product(
            document.Id,
            document.Name,
            document.Description,
            new Money(document.Price),
            document.StockQuantity,
            document.ImagePath);
    }

    private static ProductDocument ToDocument(Product product)
    {
        return new ProductDocument
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price.Amount,
            StockQuantity = product.StockQuantity,
            ImagePath = product.ImagePath
        };
    }

    private sealed class ProductDocument
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImagePath { get; set; }
    }
}
