# Frontend Architecture

The Angular app is organized by responsibility so the UI can grow without mixing API access, shared components, and feature screens.

## Folder Structure

```text
src/app/
+-- core/
|   +-- models/          # TypeScript interfaces shared across the app
|   +-- services/        # API and state services
+-- features/
|   +-- shop/            # Storefront, product list, cart, and checkout
|   +-- orders/          # Orders history screen
+-- shared/
    +-- components/      # Reusable UI components
```

## Core Services

- `ProductService`: reads products from the C# API.
- `OrderService`: sends checkout requests and reads orders.
- `CartService`: keeps client-side cart state with Angular signals.

## Routes

```text
/        Storefront and checkout
/orders  Orders history
```

## API Configuration

The frontend reads the backend URL from:

```text
src/environments/environment.ts
```

Default value:

```ts
export const environment = {
  apiUrl: 'http://localhost:5050/api'
};
```

## Styling Approach

- Global resets and base typography live in `src/styles.scss`.
- Feature-specific styles stay next to the feature component.
- Shared components own their own styles.
