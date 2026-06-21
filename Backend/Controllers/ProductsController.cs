using Ecommerce.Application.Contracts;
using Ecommerce.Application.Services;
using Ecommerce.Contracts.Requests;
using Ecommerce.Contracts.Responses;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;
using Ecommerce.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly CatalogService _catalogService;
    private readonly IProductRepository _productRepository;

    public ProductsController(CatalogService catalogService, IProductRepository productRepository)
    {
        _catalogService = catalogService;
        _productRepository = productRepository;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<ProductResponse>> GetAll()
    {
        var products = _catalogService
            .ListAvailableProducts()
            .Select(product => new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price.Amount,
                product.StockQuantity))
            .ToList()
            .AsReadOnly();

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ProductResponse> GetById(Guid id)
    {
        var product = _productRepository.GetById(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(product));
    }

    [HttpGet("admin")]
    [ServiceFilter(typeof(AdminAuthorizationFilter))]
    public ActionResult<IReadOnlyCollection<ProductResponse>> GetForManagement()
    {
        var products = _productRepository
            .GetAll()
            .OrderBy(product => product.Name)
            .Select(ToResponse)
            .ToList()
            .AsReadOnly();

        return Ok(products);
    }

    [HttpPost]
    [ServiceFilter(typeof(AdminAuthorizationFilter))]
    public ActionResult<ProductResponse> Create(ProductRequest request)
    {
        try
        {
            var product = new Product(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                new Money(request.Price),
                request.StockQuantity);

            _productRepository.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, ToResponse(product));
        }
        catch (Exception exception) when (exception is ArgumentException or ArgumentOutOfRangeException)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(AdminAuthorizationFilter))]
    public ActionResult<ProductResponse> Update(Guid id, ProductRequest request)
    {
        try
        {
            if (_productRepository.GetById(id) is null)
            {
                return NotFound();
            }

            var product = new Product(
                id,
                request.Name,
                request.Description,
                new Money(request.Price),
                request.StockQuantity);

            _productRepository.Update(product);
            return Ok(ToResponse(product));
        }
        catch (Exception exception) when (exception is ArgumentException or ArgumentOutOfRangeException or InvalidOperationException)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    [ServiceFilter(typeof(AdminAuthorizationFilter))]
    public IActionResult Delete(Guid id)
    {
        _productRepository.Delete(id);
        return NoContent();
    }

    private static ProductResponse ToResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price.Amount,
            product.StockQuantity);
    }
}
