using IceCity_Data_Management_System.Persistence.Repositories.Interfaces;

namespace IceCity_Data_Management_System.Persistence.UnitOfWork
{
    public class UnitOfWork(
        ApplicationDbContext context,
        IHouseRepository houseRepository,
        ICatShelterRepository catShelterRepository,
        ISensorReadingRepository sensorReadingRepository) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;

        // NEW — Add this so interface requirement is satisfied
        public ApplicationDbContext Context => _context;

        public IHouseRepository Houses { get; } = houseRepository;
        public ICatShelterRepository CatShelters { get; } = catShelterRepository;
        public ISensorReadingRepository SensorReadings { get; } = sensorReadingRepository;

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
