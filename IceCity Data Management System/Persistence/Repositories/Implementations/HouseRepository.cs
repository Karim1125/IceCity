using Microsoft.Extensions.Logging;

namespace IceCity_Data_Management_System.Persistence.Repositories.Implementations
{
    public class HouseRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<House>> logger)
                : GenericRepository<House>(context, logger), IHouseRepository
    {
    }
}
