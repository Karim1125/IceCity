using IceCity_Data_Management_System.Configuration;
using IceCity_Data_Management_System.Services.Interfaces;
using Serilog;

namespace IceCity_Data_Management_System.UIHelper
{
    public static class ConsoleFormatter
    {
        public static async Task RunConsoleMenuAsync(IServiceProvider provider)
        {
            var houseService = provider.GetRequiredService<IHouseService>();
            var shelterService = provider.GetRequiredService<ICatShelterService>();
            var sensorService = provider.GetRequiredService<ISensorReadingService>();
            var config = provider.GetRequiredService<AppConfig>();

            while (true)
            {
                Console.Clear();
                ConsoleFormatter.Header("ICECITY CONTROL PANEL");

                Console.WriteLine("1) List Houses");
                Console.WriteLine("2) Add House");
                Console.WriteLine("3) Update House");
                Console.WriteLine("4) Delete House");
                Console.WriteLine("5) Add Sensor Reading");
                Console.WriteLine("6) Average Outside Temperature");
                Console.WriteLine("7) Top 3 Coldest Houses (7 days)");
                Console.WriteLine("8) Count Critical Readings");
                Console.WriteLine("9) Add Cat Shelter");
                Console.WriteLine("10) Exit");
                Console.Write("Choice: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ConsoleFormatter.Header("ALL HOUSES");
                        var all = await houseService.GetAllAsync();

                        ConsoleFormatter.PrintTableHeader("ID", "Owner", "Heaters", "Hours/Day", "Temp (°C)");

                        foreach (var h in all)
                        {
                            Console.ForegroundColor = h.OutsideTemperature < -20 ? ConsoleColor.Blue : ConsoleColor.White;
                            ConsoleFormatter.PrintTableRow(h.Id, h.OwnerName, h.Heaters, h.HoursPerDay, h.OutsideTemperature);
                            Console.ResetColor();
                        }
                        break;

                    case "2":
                        ConsoleFormatter.Header("ADD NEW HOUSE");

                        Console.Write("Owner Name: ");
                        var owner = Console.ReadLine() ?? "";

                        Console.Write("Heaters: ");
                        var heaters = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Hours Per Day: ");
                        var hours = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Outside Temperature: ");
                        var temp = decimal.Parse(Console.ReadLine() ?? "0");

                        var created = await houseService.AddAsync(new CreateHouseDto
                        {
                            OwnerName = owner,
                            Heaters = heaters,
                            HoursPerDay = hours,
                            OutsideTemperature = temp
                        });

                        ConsoleFormatter.Success($"Added House ID {created.Id}");
                        break;

                    case "3":
                        ConsoleFormatter.Header("UPDATE HOUSE");

                        Console.Write("House ID: ");
                        var uid = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Heaters: ");
                        var newHeaters = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Hours Per Day: ");
                        var newHours = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Outside Temperature: ");
                        var newTemp = decimal.Parse(Console.ReadLine() ?? "0");

                        await houseService.UpdateAsync(new UpdateHouseDto
                        {
                            Id = uid,
                            Heaters = newHeaters,
                            HoursPerDay = newHours,
                            OutsideTemperature = newTemp
                        });

                        ConsoleFormatter.Success("House updated successfully.");
                        break;

                    case "4":
                        ConsoleFormatter.Header("DELETE HOUSE");

                        Console.Write("House ID: ");
                        var did = int.Parse(Console.ReadLine() ?? "0");

                        await houseService.DeleteAsync(did);
                        ConsoleFormatter.Warning("House deleted.");
                        break;

                    case "5":
                        ConsoleFormatter.Header("ADD SENSOR READING");

                        Console.Write("House ID: ");
                        var hid = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Temperature: ");
                        var st = decimal.Parse(Console.ReadLine() ?? "0");

                        await sensorService.AddAsync(new CreateSensorReadingDto
                        {
                            HouseId = hid,
                            Temperature = st,
                            Timestamp = DateTime.UtcNow
                        });

                        ConsoleFormatter.Success("Sensor reading added.");
                        break;

                    case "6":
                        var avg = await houseService.GetAverageOutsideTemperatureAsync();
                        ConsoleFormatter.Header("AVERAGE TEMPERATURE");

                        if (avg < -20)
                            ConsoleFormatter.Warning($"Average Outside Temp: {avg}°C (Extreme Cold!)");
                        else
                            ConsoleFormatter.Success($"Average Outside Temp: {avg}°C");

                        break;

                    case "7":
                        ConsoleFormatter.Header("TOP 3 COLDEST HOUSES");

                        var from = DateTime.UtcNow.AddDays(-7);
                        var to = DateTime.UtcNow;

                        var coldest = await houseService.GetTop3ColdestHousesAsync(from, to);

                        ConsoleFormatter.PrintTableHeader("Owner", "House ID");

                        foreach (var h in coldest)
                            ConsoleFormatter.PrintTableRow(h.OwnerName, h.Id);

                        break;

                    case "8":
                        ConsoleFormatter.Header("CRITICAL TEMPERATURE COUNT");

                        var count = await houseService.CountCriticalTemperatureReadingsAsync(config.CriticalTemperature);
                        ConsoleFormatter.Warning($"Readings <= {config.CriticalTemperature}: {count}");

                        break;

                    case "9":

                        ConsoleFormatter.Header("ADD CAT SHELTER");
                        try
                        {
                            Console.Write("House ID: ");
                            var shid = int.Parse(Console.ReadLine() ?? "0");

                            Console.Write("Cat Name: ");
                            var cat = Console.ReadLine() ?? "";

                            var createdShelter = await shelterService.AddAsync(new CreateCatShelterDto
                            {
                                Id = shid,
                                CatName = cat
                            });

                            ConsoleFormatter.Success($"Cat Shelter updated → {createdShelter.CatName}");
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, "Error creating CatShelter");
                            ConsoleFormatter.Error(ex.Message); 
                        }
                        break;


                    case "10":
                        return;

                    default:
                        ConsoleFormatter.Error("Invalid option.");
                        break;
                }

                Console.WriteLine("\nPress ENTER to continue...");
                Console.ReadLine();
            }
        }
        public static void Header(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n==============================================");
            Console.WriteLine($"  {title}");
            Console.WriteLine("==============================================");
            Console.ResetColor();
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintTableHeader(params string[] columns)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            foreach (var col in columns)
                Console.Write($"{col:0.00}");

            Console.WriteLine();
            Console.WriteLine(new string('-', 20 * columns.Length));

            Console.ResetColor();
        }

        public static void PrintTableRow(params object[] values)
        {
            foreach (var v in values)
                Console.Write($"{v,-20}");
            Console.WriteLine();
        }
    }
}
