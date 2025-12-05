using IceCity_Data_Management_System.Persistence.Repositories.Interfaces;

namespace IceCity_Data_Management_System.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IHouseRepository Houses { get; }
        ICatShelterRepository CatShelters { get; }
        ISensorReadingRepository SensorReadings { get; }

        ApplicationDbContext Context { get; }

        Task<int> CompleteAsync();
    }
}
