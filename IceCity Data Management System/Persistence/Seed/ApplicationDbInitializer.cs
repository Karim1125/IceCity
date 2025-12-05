namespace IceCity_Data_Management_System.Persistence.Seed
{
    public static class ApplicationDbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();
            if (!context.Houses.Any())
            {
                var houses = new List<House>
                {
                    new() {
                        OwnerName = "Aria Frost",
                        Heaters = 3,
                        HoursPerDay = 12,
                        OutsideTemperature = -25.3m
                    },
                    new() {
                        OwnerName = "Borin Icehammer",
                        Heaters = 2,
                        HoursPerDay = 10,
                        OutsideTemperature = -28.0m
                    },
                    new()
                    {
                        OwnerName = "Selene Winterborn",
                        Heaters = 4,
                        HoursPerDay = 14,
                        OutsideTemperature = -22.8m
                    },
                    new CatShelter
                    {
                        OwnerName = "Nora Snowpaw",
                        Heaters = 1,
                        HoursPerDay = 8,
                        OutsideTemperature = -30.1m,
                        CatName = "Mittens"
                    },
                    new CatShelter
                    {
                        OwnerName = "Kiro Frosttail",
                        Heaters = 2,
                        HoursPerDay = 6,
                        OutsideTemperature = -29.4m,
                        CatName = "Snowflake"
                    }
                };

                await context.Houses.AddRangeAsync(houses);
                await context.SaveChangesAsync();
            }


            if (!context.SensorReadings.Any())
            {
                var random = new Random();
                var readings = new List<SensorReading>();

                var allHouses = await context.Houses.ToListAsync();

                foreach (var house in allHouses)
                {
                    for (int i = 0; i < 5; i++) 
                    {
                        readings.Add(new SensorReading
                        {
                            HouseId = house.Id,
                            Temperature = (decimal)GenerateRandomTemperature(random),
                            Timestamp = DateTime.UtcNow.AddMinutes(-random.Next(0, 10000))
                        });
                    }
                }

                await context.SensorReadings.AddRangeAsync(readings);
                await context.SaveChangesAsync();
            }
        }

        private static double GenerateRandomTemperature(Random random)
        {
            //random temperature between -20 and -35
            double temp = -20 - random.NextDouble() * 15;
            return Math.Round((double)temp, 2);
        }
    }
}
