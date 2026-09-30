# CreditWorks Vehicle Weight Management

A fleet management web application built with **ASP.NET Core MVC** that tracks vehicles and automatically categorizes them by weight. This application allows users to manage vehicle records, view categorized fleet data, and maintain manufacturer information.

## Features

- ✅ **Vehicle Management** – Add, edit, and delete vehicle records
- ✅ **Automatic Categorization** – Vehicles automatically categorized as Light (0-500kg), Medium (500-2500kg), or Heavy (2500kg+)
- ✅ **Sortable Fleet List** – Sort vehicles by owner name, manufacturer, year, or weight
- ✅ **Manufacturer Database** – Predefined manufacturer list (Mazda, Mercedes, Honda, Ferrari, Toyota)
- ✅ **Responsive UI** – Clean, modern interface with emoji-based category icons
- ✅ **Data Validation** – Comprehensive form validation for vehicle data

## Technology Stack

- **Language:** C# (56.6%), HTML (23.5%), CSS (19.5%)
- **Framework:** ASP.NET Core MVC (net10.0)
- **Database:** SQL Server LocalDB with Entity Framework Core
- **Architecture:** Repository Pattern, Dependency Injection, MVVM

## Prerequisites

- **.NET 10 SDK** – [Download](https://dotnet.microsoft.com/download)
- **SQL Server LocalDB** – Included with Visual Studio or [download standalone](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)
- **Git** – For cloning the repository

## Project Structure

```
CreditWorksVehicleWeight/
├── Controllers/
│   ├── VehiclesController.cs      # CRUD operations (Index, Create, Edit, Delete)
│   └── HomeController.cs          # Landing page and error handling
├── Views/
│   ├── Vehicles/
│   │   ├── Index.cshtml           # Vehicle list with sortable columns
│   │   ├── Create.cshtml          # Add/edit vehicle form
│   │   └── Edit.cshtml
│   ├── Home/
│   │   ├── Index.cshtml           # Home page
│   │   └── Privacy.cshtml
│   └── Shared/
│       ├── _Layout.cshtml         # Master layout
│       └── _Layout.cshtml.css     # Shared styles
├── ViewModels/
│   ��── VehicleFormViewModel.cs    # Form binding with validation
│   └── VehicleListViewModel.cs    # List display model
├── Services/
│   └── CategoryConfigurationService.cs  # Vehicle categorization logic
├── appsettings.json               # Database connection string
├── Program.cs                     # Startup configuration
└── CreditWorksVehicleWeight.csproj

DataAccess/
├── Models/
│   ├── Vehicle.cs                 # Vehicle entity (owner, manufacturer, year, weight)
│   ├── Manufacturer.cs            # Manufacturer entity
│   └── VehicleCategory.cs         # Weight-based category (Light, Medium, Heavy)
├── DataContext/
│   ├── AppDbContext.cs            # EF Core DbContext
│   └── SeedData.cs                # Initial data seeding
├── Repository/
│   ├── Interface/
│   │   ├── IRepository.cs         # Generic CRUD interface
│   │   ├── IVehicleRepository.cs
│   │   ├── IManufacturerRepository.cs
│   │   └── IVehicleCategoryRepository.cs
│   └── Implementation/
│       ├── EfRepository.cs        # Base repository with async CRUD
│       ├── VehicleRepository.cs   # Vehicle-specific queries (sorting)
│       ├── ManufacturerRepository.cs
│       └── VehicleCategoryRepository.cs
└── DataAccess.csproj
```

## Setup & Installation

### 1. Clone the Repository

```bash
git clone https://github.com/rahulya/CreditWorksVehicleWeight.git
cd CreditWorksVehicleWeight
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

### 3. Create the Database

```bash
dotnet ef database update --project DataAccess
```

This command will:
- Create the `CreditWorksVehicles` database in LocalDB
- Apply all migrations
- Seed initial data (5 manufacturers, 3 categories)

### 4. Run the Application

```bash
dotnet run --project CreditWorksVehicleWeight
```

The application will start at: **https://localhost:5001**

## Usage

### Home Page
- Navigate to `/` to view the welcome page

### Vehicle Management
- **List Vehicles:** Visit `/Vehicles` to see all vehicles
- **Add Vehicle:** Click "＋ Add vehicle" button and fill in:
  - Owner's name (required, max 120 characters)
  - Manufacturer (required, dropdown list)
  - Year of manufacture (required, 1886-2100)
  - Weight in kg (required, 0.01-99999999.99, max 2 decimals)
- **Edit Vehicle:** Click "Edit" on any row to modify details
- **Delete Vehicle:** Click "Delete" to remove a vehicle
- **Sort:** Click column headers to sort by owner, manufacturer, year, or weight

### Vehicle Categories

Categories are automatically assigned based on weight:

| Category | Weight Range | Icon |
|----------|--------------|------|
| Light    | 0 - 500 kg   | 🛵   |
| Medium   | 500 - 2500 kg| 🚙   |
| Heavy    | 2500+ kg     | 🚚   |

If a vehicle weight falls outside all configured ranges, it's marked as "Uncategorised" (⚠️).

## Database Connection

The default connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "Vehicles": "Server=(localdb)\\MSSQLLocalDB;Database=CreditWorksVehicles;Trusted_Connection=True;TrustServerCertificate=True"
}
```

To use a different SQL Server instance, update the connection string before running migrations.

## API Endpoints

### Vehicles Controller (`/Vehicles`)

| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/Vehicles` | GET | List all vehicles (with sorting) |
| `/Vehicles?sortBy=manufacturer&direction=desc` | GET | List with custom sort |
| `/Vehicles/Create` | GET | Display add vehicle form |
| `/Vehicles/Create` | POST | Submit new vehicle |
| `/Vehicles/Edit/{id}` | GET | Display edit form |
| `/Vehicles/Edit/{id}` | POST | Submit vehicle updates |
| `/Vehicles/Delete/{id}` | POST | Delete vehicle |

### Query Parameters

- `sortBy` – `owner`, `manufacturer`, `year`, `weight` (default: `owner`)
- `direction` – `asc`, `desc` (default: `asc`)

Example: `/Vehicles?sortBy=weight&direction=desc`

## Data Validation

### Vehicle Form Validation

- **Owner Name:** Required, max 120 characters
- **Manufacturer:** Required, must exist in database
- **Year:** Required, must be between 1886 and 2100
- **Weight:** Required, positive number, max 2 decimal places, must fall within an existing category range

### Category Configuration Validation

- At least one category required
- Categories must not overlap or have gaps
- First category must start at 0 kg
- Category names must be unique (case-insensitive)
- Only allowed emoji icons: 🛵, 🚙, 🚚, 🚗, 🏎️, 🚐, 🚜, 🚛

## Architecture & Design Patterns

### Repository Pattern
- Abstraction layer for data access via `IRepository<T>` and specific interfaces
- Async/await for non-blocking database operations
- Entity Framework Core with EF.AsNoTracking() for read optimization

### Dependency Injection
- Services registered in `Program.cs`
- Constructor injection in controllers and services
- Supports testing and loose coupling

### Model-View-ViewModel (MVVM)
- `VehicleFormViewModel` – Form binding and validation
- `VehicleListViewModel` – List display with sorting metadata
- Separation of concerns between domain models and UI

### Service Layer
- `CategoryConfigurationService` – Business logic for vehicle categorization
- Reusable validation and category-finding logic

## Building for Production

```bash
dotnet publish -c Release --self-contained
```

## Troubleshooting

### Database Connection Issues
```
"Cannot open database 'CreditWorksVehicles' requested by the login..."
```
- Ensure SQL Server LocalDB is installed
- Try: `sqllocaldb info` to verify installation
- Update connection string in `appsettings.json`

### Migration Errors
```bash
dotnet ef database drop --project DataAccess
dotnet ef database update --project DataAccess
```

### Port Already in Use
If port 5001 is in use, update in `launchSettings.json` or run:
```bash
dotnet run --project CreditWorksVehicleWeight -- --urls "https://localhost:5002"
```

## Development

### Running Tests (if added)
```bash
dotnet test
```

### Entity Framework Core Commands
```bash
# Add migration
dotnet ef migrations add MigrationName --project DataAccess

# Remove last migration
dotnet ef migrations remove --project DataAccess

# View SQL
dotnet ef dbcontext scaffold
```

## Future Enhancements

- Email notifications for vehicle additions
- User authentication and authorization
- Export vehicle list to PDF/Excel
- Advanced filtering and search
- Vehicle maintenance tracking
- Driver assignment and history
- API endpoints for mobile/third-party integration

## License

This project is proprietary software for CreditWorks.

## Support

For issues or questions, please contact: yadavrahul1530@gmail.com

---

**Last Updated:** September 30, 2026  
**Framework Version:** .NET 10.0  
**Status:** Active Development
