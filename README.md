# Ventixe InvoiceService

A .NET 9 microservice for managing invoices in the Ventixe platform, providing comprehensive invoice management capabilities.

## 🌐 Live Demo

**🚀 Deployed API**: [https://invoiceservice-gmafbqd0gjg8abdf.centralus-01.azurewebsites.net](https://invoiceservice-gmafbqd0gjg8abdf.centralus-01.azurewebsites.net)

**📖 API Documentation**: [https://invoiceservice-gmafbqd0gjg8abdf.centralus-01.azurewebsites.net/swagger](https://invoiceservice-gmafbqd0gjg8abdf.centralus-01.azurewebsites.net/swagger)

## 📋 Overview

The InvoiceService is a RESTful API microservice built with .NET 9 that manages invoice operations for the Ventixe platform. It provides endpoints for creating, reading, updating, and deleting invoices with various filtering and search capabilities.

## 🛠️ Technologies Used

- **.NET 9** - Latest version of .NET framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - Object-Relational Mapping (ORM)
- **SQL Server** - Database (configurable)
- **Swagger/OpenAPI** - API documentation
- **Clean Architecture** - Layered architecture pattern

## 🏗️ Architecture

The service follows Clean Architecture principles with clear separation of concerns:

```
InvoiceService/
├── Presentation/           # API controllers and configuration
│   ├── Controllers/        # REST API controllers
│   └── Program.cs         # Application entry point
├── Application/           # Business logic and services
│   ├── Interfaces/        # Service contracts
│   ├── Models/           # DTOs and view models
│   └── Services/         # Business logic implementation
└── Persistence/          # Data access layer
    ├── Contexts/         # Entity Framework DbContext
    ├── Entities/         # Database entities
    ├── Interfaces/       # Repository contracts
    └── Repositories/     # Data access implementation
```

## 🔧 Setup Instructions

### Prerequisites

- .NET 9 SDK
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or JetBrains Rider (optional)

### Local Development

1. **Clone the repository**

   ```bash
   git clone <your-repo-url>
   cd InvoiceService
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Update database connection**
   Edit `appsettings.json` in the Presentation project:

   ```json
   {
     "ConnectionStrings": {
       "SqlConnection": "your-connection-string-here"
     }
   }
   ```

4. **Run database migrations**

   ```bash
   dotnet ef database update --project Persistence --startup-project Presentation
   ```

5. **Start the service**

   ```bash
   cd Presentation
   dotnet run
   ```

6. **Access API documentation**
   Navigate to `https://localhost:7001/swagger` (or the displayed URL)

## 📡 API Endpoints

### Invoice Management

| Method   | Endpoint                               | Description             |
| -------- | -------------------------------------- | ----------------------- |
| `GET`    | `/api/invoices`                        | Get all invoices        |
| `GET`    | `/api/invoices/{id}`                   | Get invoice by ID       |
| `GET`    | `/api/invoices/number/{invoiceNumber}` | Get invoice by number   |
| `GET`    | `/api/invoices/status/{status}`        | Get invoices by status  |
| `GET`    | `/api/invoices/overdue`                | Get overdue invoices    |
| `GET`    | `/api/invoices/event/{eventId}`        | Get invoices by event   |
| `GET`    | `/api/invoices/user/{userId}`          | Get invoices by user    |
| `POST`   | `/api/invoices`                        | Create new invoice      |
| `PUT`    | `/api/invoices/{id}`                   | Update existing invoice |
| `DELETE` | `/api/invoices/{id}`                   | Delete invoice          |

### Request/Response Examples

#### Get All Invoices

```http
GET /api/invoices
```

**Response:**

```json
{
  "success": true,
  "result": [
    {
      "id": "11111111-1111-1111-1111-111111111111",
      "invoiceNumber": "INV1981",
      "eventId": "22222222-2222-2222-2222-222222222222",
      "eventName": "Echo Beats Festival",
      "userId": "33333333-3333-3333-3333-333333333333",
      "userName": "Jackson Moore",
      "amount": 654.0,
      "issueDate": "2025-05-20T00:00:00",
      "dueDate": "2025-06-15T00:00:00",
      "status": "Overdue",
      "description": "Event ticket payment",
      "createdAt": "2025-04-28T00:00:00"
    }
  ]
}
```

#### Create Invoice

```http
POST /api/invoices
Content-Type: application/json

{
  "eventId": "22222222-2222-2222-2222-222222222222",
  "userId": "33333333-3333-3333-3333-333333333333",
  "amount": 500.00,
  "dueDate": "2024-12-31T00:00:00",
  "description": "Service payment"
}
```

#### Update Invoice

```http
PUT /api/invoices/{id}
Content-Type: application/json

{
  "amount": 750.00,
  "dueDate": "2024-12-31T00:00:00",
  "status": "Paid",
  "description": "Updated service payment"
}
```

## 🗃️ Data Models

### InvoiceDto

```csharp
public class InvoiceDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; }
    public Guid EventId { get; set; }
    public string EventName { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public decimal Amount { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } // "Pending", "Paid", "Overdue"
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### CreateInvoiceDto

```csharp
public class CreateInvoiceDto
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Description { get; set; }
}
```

## 🚀 Deployment

### Azure App Service

The service is deployed to Azure App Service with the following configuration:

- **Platform**: Azure App Service (Windows)
- **Runtime**: .NET 9
- **Database**: Azure SQL Database (or SQL Server)
- **CORS**: Enabled for cross-origin requests

### Deployment Commands

```bash
# Build for production
dotnet publish Presentation/Presentation.csproj -c Release -o ./publish

# Deploy to Azure (using Azure CLI)
az webapp deployment source config-zip \
  --name invoiceservice-gmafbqd0gjg8abdf \
  --resource-group your-resource-group \
  --src publish.zip
```

### Environment Configuration

Set the following application settings in Azure:

```json
{
  "ConnectionStrings__SqlConnection": "your-azure-sql-connection-string",
  "ASPNETCORE_ENVIRONMENT": "Production"
}
```

## 🔒 Security Features

- **CORS Configuration**: Allows cross-origin requests from frontend
- **Input Validation**: Model validation on all endpoints
- **Error Handling**: Consistent error responses
- **HTTPS**: Enforced in production

## 🧪 Testing

### Run Unit Tests

```bash
dotnet test
```

### API Testing with curl

```bash
# Get all invoices
curl -X GET "https://invoiceservice-gmafbqd0gjg8abdf.centralus-01.azurewebsites.net/api/invoices"

# Get overdue invoices
curl -X GET "https://invoiceservice-gmafbqd0gjg8abdf.centralus-01.azurewebsites.net/api/invoices/overdue"

# Create new invoice
curl -X POST "https://invoiceservice-gmafbqd0gjg8abdf.centralus-01.azurewebsites.net/api/invoices" \
  -H "Content-Type: application/json" \
  -d '{
    "eventId": "22222222-2222-2222-2222-222222222222",
    "userId": "33333333-3333-3333-3333-333333333333",
    "amount": 500.00,
    "dueDate": "2024-12-31T00:00:00",
    "description": "Test invoice"
  }'
```

## 📊 Status Codes

| Status    | Description                         |
| --------- | ----------------------------------- |
| `Pending` | Invoice created but not yet paid    |
| `Paid`    | Invoice has been paid               |
| `Overdue` | Invoice is past due date and unpaid |

## 🔄 Integration

### Frontend Integration

The service integrates with the Ventixe Frontend application:

```javascript
// Frontend service usage
import { invoiceService } from "./services/invoiceService";

// Get all invoices
const invoices = await invoiceService.getAllInvoices();

// Create invoice
const newInvoice = await invoiceService.createInvoice({
  eventId: "event-guid",
  userId: "user-guid",
  amount: 500.0,
  dueDate: "2024-12-31T00:00:00",
  description: "Service payment",
});
```

## 🔧 Configuration

### Development Configuration (appsettings.Development.json)

```json
{
  "ConnectionStrings": {
    "SqlConnection": "Server=(localdb)\\mssqllocaldb;Database=VentixeInvoiceDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Production Configuration

```json
{
  "ConnectionStrings": {
    "SqlConnection": "your-production-connection-string"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
```

## 🐛 Troubleshooting

### Common Issues

1. **Database Connection Errors**

   - Verify connection string in appsettings.json
   - Ensure SQL Server is running
   - Check firewall settings for Azure SQL

2. **CORS Errors**

   - Verify CORS is configured in Program.cs
   - Check allowed origins in production

3. **Migration Issues**
   ```bash
   dotnet ef migrations add InitialCreate --project Persistence --startup-project Presentation
   dotnet ef database update --project Persistence --startup-project Presentation
   ```

## 📈 Performance Considerations

- **Async/Await**: All database operations are asynchronous
- **Response Caching**: Consider implementing for read-heavy operations
- **Pagination**: Implement for large result sets
- **Indexing**: Ensure proper database indexes on frequently queried fields

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Support

For support and questions:

- Check the API documentation at `/swagger`
- Create an issue in the GitHub repository
- Contact the development team

---

Built with ❤️ using .NET 9 and deployed on Azure App Service
