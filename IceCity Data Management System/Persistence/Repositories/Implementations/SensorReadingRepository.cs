using Microsoft.Extensions.Logging;

namespace IceCity_Data_Management_System.Persistence.Repositories.Implementations
{
    public class SensorReadingRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<SensorReading>> logger)
                : GenericRepository<SensorReading>(context, logger), ISensorReadingRepository
    {
    }
}
