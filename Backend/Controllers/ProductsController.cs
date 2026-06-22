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
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    private const long MaxImageSizeInBytes = 5 * 1024 * 1024;

    private readonly CatalogService _catalogService;
    private readonly IProductRepository _productRepository;
    private readonly IWebHostEnvironment _environment;

    public ProductsController(
        CatalogService catalogService,
        IProductRepository productRepository,
        IWebHostEnvironment environment)
    {
        _catalogService = catalogService;
        _productRepository = productRepository;
        _environment = environment;
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
                product.StockQuantity,
                product.ImagePath ?? "/images/NoImage.png"))
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
            var existingProduct = _productRepository.GetById(id);

            if (existingProduct is null)
            {
                return NotFound();
            }

            var product = new Product(
                id,
                request.Name,
                request.Description,
                new Money(request.Price),
                request.StockQuantity,
                existingProduct.ImagePath);

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

    [HttpPost("{id:guid}/image")]
    [ServiceFilter(typeof(AdminAuthorizationFilter))]
    [RequestSizeLimit(MaxImageSizeInBytes)]
    public async Task<ActionResult<ProductResponse>> UploadImage(Guid id, [FromForm] IFormFile image)
    {
        var product = _productRepository.GetById(id);

        if (product is null)
        {
            return NotFound();
        }

        if (image.Length <= 0)
        {
            return BadRequest("Select an image to upload.");
        }

        if (image.Length > MaxImageSizeInBytes)
        {
            return BadRequest("Image must be 5 MB or smaller.");
        }

        var extension = Path.GetExtension(image.FileName);

        if (!AllowedImageExtensions.Contains(extension))
        {
            return BadRequest("Only JPG, PNG, and WEBP images are allowed.");
        }

        var contentType = image.ContentType.ToLowerInvariant();

        if (!contentType.StartsWith("image/"))
        {
            return BadRequest("The uploaded file must be an image.");
        }

        var fileName = $"{id:N}-{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
        var uploadFolder = Path.Combine(webRootPath, "uploads", "products");
        Directory.CreateDirectory(uploadFolder);

        var filePath = Path.Combine(uploadFolder, fileName);

        await using (var stream = System.IO.File.Create(filePath))
        {
            await image.CopyToAsync(stream);
        }

        var imagePath = $"/uploads/products/{fileName}";
        var updatedProduct = new Product(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.StockQuantity,
            imagePath);

        _productRepository.Update(updatedProduct);

        return Ok(ToResponse(updatedProduct));
    }

    private static ProductResponse ToResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price.Amount,
            product.StockQuantity,
            product.ImagePath ?? "/images/NoImage.png");
    }
}
