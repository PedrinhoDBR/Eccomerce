# E-commerce API

C# ASP.NET Core Web API for the e-commerce application.

## Requirements

- .NET SDK 6.0 or later.

## How to Run

```bash
dotnet run --project Ecommerce.Api.csproj
```

The API runs at:

```text
http://localhost:5050
```

Swagger is available in development at:

```text
http://localhost:5050/swagger
```

## Endpoints

```text
GET  /api/products
GET  /api/orders
POST /api/cart/checkout
```

## Checkout Payload

```json
{
  "customerName": "Ada Lovelace",
  "customerEmail": "ada@example.com",
  "items": [
    {
      "productId": "1c4f8f15-6e0f-4c1c-9c6e-0ff2d661f101",
      "quantity": 1
    }
  ]
}
```

## Folder Structure

```text
Backend/
+-- Application/
|   +-- Contracts/       # Repository interfaces used by use cases
|   +-- Services/        # Application use cases
+-- Contracts/
|   +-- Requests/        # API request DTOs
|   +-- Responses/       # API response DTOs
+-- Controllers/         # HTTP endpoints
+-- Domain/
|   +-- Entities/        # Business entities
|   +-- Enums/           # Domain enumerations
|   +-- ValueObjects/    # Value objects
+-- Infrastructure/
|   +-- Repositories/    # In-memory persistence
+-- Docs/
    +-- ARCHITECTURE.md  # Backend architecture notes
```

## Design Notes

- The API exposes DTOs instead of returning domain entities directly.
- Domain classes protect their own invariants.
- Application services contain use-case orchestration.
- Infrastructure is behind interfaces, so in-memory repositories can be replaced by Entity Framework later.
- CORS is configured for the Angular client at `http://localhost:4200`.
