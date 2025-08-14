# Shop.Net

A comprehensive Point of Sale (POS) and Inventory Management System built with .NET 9 Web API. This system provides a complete solution for retail businesses to manage products, process sales, track inventory, and generate detailed reports.

## Project Structure

```
TodoApi/
├── Controllers/           # API Controllers
│   ├── POSController.cs
│   ├── InventoryController.cs
│   ├── ProductsController.cs
│   ├── ReportsController.cs
│   └── DashboardController.cs
├── Services/              # Business Logic Layer
│   ├── Interfaces/
│   ├── POSService.cs
│   ├── InventoryService.cs
│   └── ReportService.cs
├── Repositories/          # Data Access Layer
│   ├── Interfaces/
│   ├── ProductRepository.cs
│   ├── SaleRepository.cs
│   └── InventoryRepository.cs
├── Models/
│   ├── Entities/          # Database Entities
│   ├── DTOs/              # Data Transfer Objects
│   └── Enums/             # Enumerations
├── Data/                  # Database Context
│   └── ShopDbContext.cs
└── Migrations/            # EF Core Migrations
```

## Database Schema

The system uses the following main entities:

- **Product**: Product information and pricing
- **Sale**: Sales transactions
- **SaleItem**: Individual items in a sale
- **StockMovement**: Inventory movement tracking
- **Recipe**: Product composition for complex items
- **Supplier**: Supplier information
