using Microsoft.Extensions.Logging;

namespace IceCity_Data_Management_System.Persistence.Repositories.Implementations
{
    public class CatShelterRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<CatShelter>> logger)
                : GenericRepository<CatShelter>(context, logger), ICatShelterRepository
    {
    }
}
