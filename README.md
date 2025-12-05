# IceCity Backend System

This repository contains the backend simulation for the **IceCity Blizzard Control Department**, implemented in C# using .NET and Entity Framework Core.
The system manages residential houses, cat shelters, temperature sensors, and analytical reporting used to monitor dangerous cold conditions across IceCity.

The project demonstrates a structured backend architecture including:

* EF Core Code-First modeling
* Repository and Unit of Work patterns
* DTO-based service layer
* AutoMapper
* Singleton configuration provider
* Text-file logging using Serilog
* Console UI with formatted output

---

## 1. System Overview

Due to extreme environmental conditions, IceCity requires a backend capable of:

* Tracking houses and heater activity
* Monitoring temperature readings from sensors
* Managing CatShelters (specialized houses with CatName)
* Running analytics to detect dangerous cold zones
* Logging all internal data operations

This project provides a clean implementation of that system in a console application.

---

## 2. Domain Model (EF Core)

### House

```text
Id                  int
OwnerName           string
Heaters             int
HoursPerDay         int
OutsideTemperature  decimal
HouseType           string (discriminator)
```

### CatShelter (inherits House)

```text
CatName             string
```

### SensorReading

```text
Id                  int
HouseId             int (FK)
Temperature         decimal
Timestamp           DateTime
```

A TPH (Table-Per-Hierarchy) inheritance strategy is used for House/CatShelter.

---

## 3. Configuration (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=IceCityDB;Trusted_Connection=True;"
  },

  "IceCitySettings": {
    "CriticalTemperature": -15.0,
    "LogsFolder": "Logs"
  },

  "Serilog": {
    "Using": [ "Serilog.Sinks.File" ],
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "Logs/icecity-db.log",
          "rollingInterval": "Day"
        }
      }
    ]
  }
}
```

---

## 4. Project Structure

```
/Configuration
    AppConfig.cs

/DTOs
    House DTOs
    Sensor DTOs
    CatShelter DTOs

/Persistence
    ApplicationDbContext.cs
    /Migrations
    /Repositories
        IRepository.cs
        GenericRepository.cs
        HouseRepository.cs
        CatShelterRepository.cs
        SensorReadingRepository.cs
    /UnitOfWork
        IUnitOfWork.cs
        UnitOfWork.cs

/Services
    /Interfaces
    /Implementations
        HouseService.cs
        CatShelterService.cs
        SensorReadingService.cs

/UIHelper
    ConsoleFormatter.cs              (formatted table printing, colors)

/Logs
    icecity-db.log                   (created at runtime)

Program.cs
README.md
appsettings.json
```

---

## 5. Running EF Core Migrations

### Add migration

```
dotnet ef migrations add InitialCreate
```

### Apply migration

```
dotnet ef database update
```

If using Visual Studio, ensure the console app is set as the startup project.

---

## 6. Running the Program

```
dotnet run
```

On startup, the system:

1. Applies migrations
2. Seeds 5 Houses + 20 SensorReadings
3. Initializes logging
4. Displays the console control panel

---

## 7. Console Menu

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

## 8. Example Console Output

### House Table Example

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

Houses below –20°C are highlighted using console colors.

---

## 9. Logging

All operations are logged to:

```
Logs/icecity-db.log
```

Each entry includes:

* Timestamp
* Log level
* Operation name
* Entity identifier
* Success/failure

### Example Log Output

```
[2025-12-05 01:39:12] [Information] [12/05/2025 01:39:12] "Update CatShelter succeeded"
[2025-12-05 01:45:29] [Information] [12/05/2025 01:45:29] "GetAll House"
[2025-12-05 02:02:54] [Information] [12/05/2025 02:02:54] "Get House by Id=2 → FOUND"
[2025-12-05 02:02:54] [Information] [12/05/2025 02:02:54] "Update House succeeded"
```

### Example Error Log

```
[2025-12-05 01:26:27] [Error] [12/05/2025 01:26:27] "Add CatShelter FAILED"
System.InvalidOperationException: The instance of entity type 'CatShelter' cannot be tracked...
```

---

## 10. Analytical Queries (LINQ)

The Services layer implements:

* **Average outside temperature**
* **Houses with heaters above a threshold**
* **Top 3 coldest houses in a time window**
* **Critical temperature readings count**
* **Updating house temperature based on all sensor readings**

This demonstrates EF Core querying, grouping, ordering, and aggregation.

---

## 12. How to Run

1. Clone repository
2. Set connection string in `appsettings.json`
3. Run migrations:

   ```
   dotnet ef database update
   ```
4. Run program:

   ```
   dotnet run
   ```

---