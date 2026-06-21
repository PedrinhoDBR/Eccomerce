# Backend Architecture

The backend follows a small layered architecture that keeps business rules separate from HTTP and persistence details.

## Layers

- `Domain`: entities, enums, and value objects that model the business.
- `Application`: use cases and contracts. This layer coordinates business operations without knowing how data is stored.
- `Infrastructure`: concrete repository implementations. The current implementation stores data in memory.
- `Contracts`: request and response DTOs used by the API.
- `Controllers`: ASP.NET Core controllers that translate HTTP requests into application use cases.

## Request Flow

1. Angular sends an HTTP request to an API controller.
2. The controller validates transport-level concerns and maps data to domain objects.
3. An application service runs the use case.
4. Repositories load or store data through interfaces.
5. The controller maps the result to a response DTO.

## Main Use Cases

- `CatalogService`: lists available products.
- `CheckoutService`: validates stock, creates an order, decreases stock, and stores the order.

## Extension Points

- Replace `InMemoryProductRepository` and `InMemoryOrderRepository` with Entity Framework repositories.
- Add authentication and authorization.
- Add payment and shipping services behind interfaces.
- Add automated tests for services and controllers.
