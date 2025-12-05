namespace IceCity_Data_Management_System.Services.Interfaces
{
    public interface ISensorReadingService
    {
        Task<List<SensorReadingDto>> GetAllAsync();
        Task<List<SensorReadingDto>> GetByHouseIdAsync(int houseId);
        Task<SensorReadingDto> AddAsync(CreateSensorReadingDto dto);
        Task<bool> DeleteAsync(int id);

        // queries
        Task<decimal> GetAverageForHouseAsync(int houseId);
        Task<int> CountReadingsBelowAsync(decimal threshold);
    }
}
