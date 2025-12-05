namespace IceCity_Data_Management_System.Services.Interfaces
{
    public interface IHouseService
    {
        Task<List<HouseDto>> GetAllAsync();
        Task<HouseDto?> GetByIdAsync(int id);
        Task<HouseDto> AddAsync(CreateHouseDto dto);
        Task<bool> UpdateAsync(UpdateHouseDto dto);
        Task<bool> UpdateTemperatureFromSensorsAsync(int houseId, int lastNReadings = 5);
        Task<bool> DeleteAsync(int id);

        // Queries
        Task<decimal> GetAverageOutsideTemperatureAsync();
        Task<List<HouseDto>> GetHousesWithHeatersAboveAsync(int threshold);
        Task<List<HouseDto>> GetTop3ColdestHousesAsync(DateTime from, DateTime to);
        Task<int> CountCriticalTemperatureReadingsAsync(decimal criticalThreshold);
    }
}
