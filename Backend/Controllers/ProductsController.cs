using Ecommerce.Application.Services;
using Ecommerce.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly CatalogService _catalogService;

    public ProductsController(CatalogService catalogService)
    {
        _catalogService = catalogService;
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
}
