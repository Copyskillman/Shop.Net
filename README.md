# Shop.Net

A comprehensive Point of Sale (POS) and Inventory Management System built with .NET 8 Web API. This system provides a complete solution for retail businesses to manage products, process sales, track inventory, and generate detailed reports.

## Features

### 🛒 Point of Sale (POS)

- Process sales transactions with multiple payment methods
- Real-time inventory updates during sales
- Support for cash, credit card, and digital payments
- Transaction history and receipt generation

### 📦 Inventory Management

- Real-time stock tracking and monitoring
- Automated low stock alerts
- Stock movement history with detailed logs
- Supplier management integration
- Product categorization and organization

### 📊 Product Management

- Complete CRUD operations for products
- Product categorization and pricing
- Recipe management for composite products
- Barcode support and product search
- Bulk product operations

### 📈 Reporting & Analytics

- Sales reports with date range filtering
- Inventory status reports
- Top-selling products analysis
- Revenue tracking and analytics
- Dashboard with key performance indicators

### 🎛️ Dashboard

- Real-time business overview
- Key metrics and KPIs
- Quick access to important functions
- Visual data representation

## Technology Stack

- **Framework**: .NET 8 Web API
- **Database**: Entity Framework Core with SQL Server
- **Architecture**: Clean Architecture with Repository Pattern
- **Authentication**: JWT Bearer Token (ready for implementation)
- **Documentation**: Swagger/OpenAPI
- **Testing**: Ready for unit and integration tests

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

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository

```bash
git clone https://github.com/Copyskillman/Shop.Net.git
cd Shop.Net
```

2. Restore NuGet packages

```bash
dotnet restore
```

3. Update the connection string in `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ShopNetDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

4. Run database migrations

```bash
dotnet ef database update
```

5. Run the application

```bash
dotnet run
```

The API will be available at `https://localhost:7071` with Swagger documentation at `https://localhost:7071/swagger`

## API Endpoints

### Products

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### POS (Point of Sale)

- `POST /api/pos/sale` - Process a sale
- `GET /api/pos/sales` - Get sales history
- `GET /api/pos/sales/{id}` - Get sale details

### Inventory

- `GET /api/inventory` - Get inventory status
- `POST /api/inventory/adjust` - Adjust stock levels
- `GET /api/inventory/movements` - Get stock movement history
- `GET /api/inventory/low-stock` - Get low stock alerts

### Reports

- `GET /api/reports/sales` - Sales reports
- `GET /api/reports/inventory` - Inventory reports
- `GET /api/reports/top-products` - Top selling products

### Dashboard

- `GET /api/dashboard/overview` - Dashboard overview data

## Database Schema

The system uses the following main entities:

- **Product**: Product information and pricing
- **Sale**: Sales transactions
- **SaleItem**: Individual items in a sale
- **StockMovement**: Inventory movement tracking
- **Recipe**: Product composition for complex items
- **Supplier**: Supplier information

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Future Enhancements

- [ ] User authentication and authorization
- [ ] Multi-store support
- [ ] Mobile app integration
- [ ] Barcode scanning
- [ ] Email notifications
- [ ] Advanced reporting with charts
- [ ] Integration with payment gateways
- [ ] Customer management system

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For support and questions, please open an issue in the GitHub repository.
