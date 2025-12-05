# IceCity Backend System

A backend simulation for **IceCity’s Blizzard Control Department**, implemented as a C# .NET console application.
The system manages houses, cat shelters, temperature sensor readings, and analytical queries required for monitoring the city during extreme blizzard conditions.

The application uses Entity Framework Core, Repository and Unit of Work patterns, AutoMapper, structured logging, and a formatted console UI.

---

## 1. Technology Requirements

```
.NET SDK: 10.0.100
C# Language Version: 12
Entity Framework Core: 8.x
Database: SQL Server LocalDB or SQL Server Express
Serilog: File and Console Sinks
```

Check your .NET version:

```
dotnet --version
```

---

## 2. Project Architecture Overview

```
/Configuration
    AppConfig.cs

/Contractss
    House, CatShelter, Sensor DTOs

/Mapping
    AutoMapper profiles

/Persistence
    ApplicationDbContext.cs
    DesignTimeDbContextFactory.cs
    /EntitiesConfigurations
    /Migrations
        HouseConfigurations.cs
        CatShelterConfiguration.cs
        SensorReadingConfiguration.cs
    /Repositories
        IRepository.cs
        GenericRepository.cs
        HouseRepository.cs
        CatShelterRepository.cs
        SensorReadingRepository.cs
    /UnitOfWork
        IUnitOfWork.cs
        UnitOfWork.cs
    /Seed
        ApplicationDbInitializer.cs

/Services
    Interfaces
    Implementations:
        HouseService.cs
        CatShelterService.cs
        SensorReadingService.cs

/UIHelper ( By AI )
    ConsoleFormatter.cs

/Logs
    icecity-db.log

Program.cs
appsettings.json
README.md
```

---

## 3. Domain Models

### House (Base)
├── Id                  : int
├── OwnerName           : string
├── Heaters             : int
├── HoursPerDay         : int
├── OutsideTemperature  : decimal
└── Discriminator       : string (EF Core inheritance)


### CatShelter : House
└── CatName             : string

### SensorReading
├── Id          : int
├── HouseId     : int (FK)
├── Temperature : decimal
└── Timestamp   : DateTime



---

## 4. appsettings.json

Readable and production-ready:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Integrated Security = SSPI ; TrustServerCertificate = True;"
  },

  "IceCitySettings": {
    "CriticalTemperature": -15.0
  },

  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "D:\\.Net\\CIS\\IceCity Data Management System\\Logs/icecity-log-.txt",
          "rollingInterval": "Day",
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] {Message}{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": [ "FromLogContext" ],
    "Properties": {
      "ApplicationName": "IceCity"
    }
  },

  "AllowedHosts": "*"
}

```

---

## 5. Database Setup

### Create Migrations

```
dotnet ef migrations add InitialCreate
```

### Update Database

```
dotnet ef database update
```

The first application run will also seed:

* 5 houses
* 20+ temperature sensor readings

---

## 6. Application Features

The console application demonstrates:

1. Database initialization and seeding
2. CRUD operations for Houses and Cat Shelters
3. Adding sensor readings
4. Temperature analysis using LINQ:

   * Average outside temperature
   * Houses with heaters above a threshold
   * Top 3 coldest houses (last 7 days)
   * Count of critical readings
5. Updating a house temperature based on all sensor readings
6. Logging every action to a text file
7. Structured, formatted console UI

---

## 7. Console Menu ( By AI )

```
ICECITY CONTROL PANEL
1) List Houses
2) Add House
3) Update House
4) Delete House
5) Add Sensor Reading
6) Average Outside Temperature
7) Top 3 Coldest Houses (7 days)
8) Count Critical Readings
9) Add Cat Shelter
10) Update Temperature from Sensor Readings
11) Exit
```

---

## 8. Expected Console Output (Formatted Table)

When selecting **List Houses**, the output appears as:

```
----------------------------------------------------------------------------------------
ID                  Owner               Heaters             Hours/Day           Temp (°C)
----------------------------------------------------------------------------------------
1                   Nora Snowpaw        1                   8                   -30.10
2                   Kiro Frosttail      2                   6                   -29.40
3                   Aria Frost          3                   12                  -25.30
4                   Borin Icehammer     2                   10                  -28.00
5                   Selene Winterborn   4                   14                  -22.80
```

Output is aligned using fixed column widths and color-highlighted for extreme temperatures.

---

## 9. Logging

All operations are logged to:

```
Logs/icecity-db.log
```

Every log entry includes:

* Timestamp
* Level
* Operation
* Entity and key
* Result or exception

### Example Log Output

```
[2025-12-05 01:39:12] [Information] [12/05/2025 01:39:12] "Update CatShelter succeeded"
[2025-12-05 01:45:51] [Information] [12/05/2025 01:45:51] "GetAllQueryable House"
[2025-12-05 02:02:54] [Information] [12/05/2025 02:02:54] "Get House by Id=2 → FOUND"
[2025-12-05 02:02:54] [Information] [12/05/2025 02:02:54] "Update House succeeded"
```

### Example Error Log

```
[2025-12-05 01:26:27] [Error] "Add CatShelter FAILED"
System.InvalidOperationException: The instance of entity type 'CatShelter' cannot be tracked...
```

---

## 10. Running the Application

```
dotnet run
```

After launching:

* The database initializes
* Seed data is inserted
* The console menu opens

The system is ready for interactive operations.

If you'd like, I can also generate:

* A printable PDF version of this README
* A project architecture diagram
* A GitHub Wiki version
* A UML class diagram for your documentation
