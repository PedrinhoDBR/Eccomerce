# E-commerce Web Application

Full-stack e-commerce study project with an Angular frontend and a C# ASP.NET Core backend.

## Applications

- `Backend`: C# ASP.NET Core Web API.
- `Frontend`: Angular storefront.

## Run Locally

Start the backend:

```bash
cd Backend
dotnet run --project Ecommerce.Api.csproj
```

Start the frontend in another terminal:

```bash
cd Frontend
npm start
```

Open:

```text
http://localhost:4200
```

## Local URLs

```text
Frontend: http://localhost:4200
Backend:  http://localhost:5050
Swagger:  http://localhost:5050/swagger
```

## Repository Structure

```text
Study_C#/
+-- Backend/
|   +-- Application/
|   +-- Contracts/
|   +-- Controllers/
|   +-- Domain/
|   +-- Infrastructure/
|   +-- Docs/
+-- Frontend/
    +-- docs/
    +-- src/app/core/
    +-- src/app/features/
    +-- src/app/shared/
```
