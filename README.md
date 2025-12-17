InventoryApp (Console + API) — .NET 8 + SQL Server
InventoryApp is a small inventory management solution built with C# / .NET 8, offering two ways to interact with products:

InventoryApp.Console: a console (CLI) application with a menu for CRUD operations.
InventoryApp.Api: an ASP.NET Core Web API exposing REST endpoints for products (with Swagger in Development).

The solution follows a simple layered architecture:
Models (entities)
Data/Repository (ADO.NET + Microsoft.Data.SqlClient)
Services/BL (business logic)

---

Tech Stack
.NET 8.0
ASP.NET Core Web API
SQL Server (via Microsoft.Data.SqlClient)
Swagger / OpenAPI (Swashbuckle.AspNetCore)

---

Repository Structure
InventoryApp.Console.sln
├── InventoryApp.Api
│ ├── Controllers
│ │ └── ProductsController.cs
│ ├── Program.cs
│ ├── appsettings.json
│ └── Properties/launchSettings.json
├── InventoryApp.Console
│ └── Program.cs
├── InventoryApp.Data
│ ├── Helper/DbConnectionHelper.cs
│ ├── Interfaces/IProductRepository.cs
│ └── Repositories/ProductRepository.cs
├── InventoryApp.Models
│ └── Models/Product.cs
└── InventoryApp.Services
└── Services/ProductService.cs

Requirements
.NET SDK 8
SQL Server (Express/Developer/LocalDB are fine)
(Optional) SQL Server Management Studio (SSMS)

---

Database Setup (SQL Server)
This project expects a database named InventoryDB and a table named Products.

Suggested SQL script
```sql
CREATE DATABASE InventoryDB;
GO

USE InventoryDB;
GO

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL
);
GO
```
Configuration (Connection String)
API

In InventoryApp.Api/appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

Update Server=... to match your SQL Server instance.

Console

The Console app uses a connection string via DbConnectionHelper and may be hardcoded depending on your setup.
Update it to match your local SQL Server configuration.

Recommendation: move the Console connection string to appsettings.json or environment variables to avoid editing code.
-----------------------------------------------------------------------------------------------------------------------------------------------
How to Run

From the repository root:
dotnet restore
dotnet build

Run the API
dotnet run --project InventoryApp.Api


Swagger (default dev URLs):

http://localhost:5249/swagger

https://localhost:7140/swagger

Run the Console App (CLI)

In a separate terminal:

dotnet run --project InventoryApp.Console
----------------------------------------------------------------------------------------------------------------------------------------------
Console App Usage

When you run the console app, you’ll see a menu similar to:

1 - Add Product

2 - Show All Products

3 - Show Product by ID

4 - Update Product

5 - Remove Product

6 - Exit

The flow is:
Console -> ProductService -> ProductRepository -> SQL Server

----------------------------------------------------------------------------------------------------------------------------------------------
API Endpoints

Base URL (by default):

http://localhost:5249/

Products (/api/products)

GET /api/products
Returns a list of products, or 404 if none exist.

GET /api/products/{id}
Returns a product by id, or 400 if the id is invalid.

POST /api/products
JSON body:
{ "name": "Keyboard", "price": 49.99, "quantity": 10 }

Creates a product and returns it with the assigned Id.

PUT /api/products
JSON body:
{ "id": 1, "name": "Keyboard Pro", "price": 59.99, "quantity": 8 }

Updates a product.

DELETE /api/products/{id}
Returns 204 NoContent on success.

Minor note: the controller uses HttpGetAttribute("{id}") for the GET-by-id route (it works, but the common style is [HttpGet("{id}")]).
-----------------------------------------------------------------------------------------------------------------------------------------------

Implementation Notes (Data Layer)

The repository uses parameterized ADO.NET queries such as:

INSERT ... SELECT CAST(SCOPE_IDENTITY() AS int);

SELECT Id, Name, Price, Quantity FROM Products

UPDATE Products SET ... WHERE Id=@Id

DELETE FROM Products WHERE Id=@Id

SELECT TOP 1 ... ORDER BY Id DESC

------------------------------------------------------------------------------------------------------------------------------------------------
Possible Improvements

Remove the API’s ProjectReference to the Console project (usually unnecessary).

Unify configuration so both API and Console read connection strings from configuration files/env vars.

Improve REST conventions (e.g., CreatedAtAction(...) on POST).

Add unit tests for ProductService and integration tests for the repository.

------------------------------------------------------------------------------------------------------------------------------------------------

License

No license is defined yet. If you plan to open-source the project, consider adding a LICENSE file (MIT is a common choice).


