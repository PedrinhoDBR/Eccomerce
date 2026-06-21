# E-commerce Frontend

Angular frontend for the C# e-commerce API.

## Requirements

- Node.js 22 or later.
- npm 10 or later.
- Backend running at `http://localhost:5050`.

## Install Dependencies

```bash
npm install
```

## Run the App

```bash
npm start
```

The Angular app runs at:

```text
http://localhost:4200
```

## Build

```bash
npm run build
```

## Features

- Product catalog.
- Client-side cart state.
- Quantity editing.
- Checkout form.
- Orders history page.
- API error handling for backend availability and checkout validation.

## Folder Structure

```text
Frontend/
+-- docs/
|   +-- ARCHITECTURE.md
+-- src/
    +-- app/
        +-- core/
        +-- features/
        +-- shared/
```

See [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) for more details.
