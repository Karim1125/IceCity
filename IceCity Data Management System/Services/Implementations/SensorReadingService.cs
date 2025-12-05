using IceCity_Data_Management_System.Services.Interfaces;

namespace IceCity_Data_Management_System.Services.Implementations
{
    public class SensorReadingService(IUnitOfWork uow, IMapper mapper) : ISensorReadingService
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        public async Task<List<SensorReadingDto>> GetAllAsync()
        {
            var readings = await _uow.SensorReadings.GetAllAsync();
            return _mapper.Map<List<SensorReadingDto>>(readings);
        }

        public async Task<List<SensorReadingDto>> GetByHouseIdAsync(int houseId)
        {
            var list = await _uow.SensorReadings
                .GetAllQueryable()
                .Where(r => r.HouseId == houseId)
                .ToListAsync();

            return _mapper.Map<List<SensorReadingDto>>(list);
        }

        public async Task<SensorReadingDto> AddAsync(CreateSensorReadingDto dto)
        {
            var sensor = _mapper.Map<SensorReading>(dto);

            await _uow.SensorReadings.AddAsync(sensor);
            await _uow.CompleteAsync();

            // update house temp
            await RecalculateHouseTemperature(dto.HouseId);

            return _mapper.Map<SensorReadingDto>(sensor);
        }

        private async Task RecalculateHouseTemperature(int houseId)
        {
            var house = await _uow.Houses.GetByIdAsync(houseId);
            if (house == null)
                return;

            var temps = await _uow.SensorReadings
                .GetAllQueryable()
                .Where(r => r.HouseId == houseId)
                .Select(r => r.Temperature)
                .ToListAsync();

            if (temps.Count == 0)
                return;

            house.OutsideTemperature = (decimal)temps.Average();

            _uow.Houses.Update(house);
            await _uow.CompleteAsync();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var reading = await _uow.SensorReadings.GetByIdAsync(id);
            if (reading == null)
                return false;

            _uow.SensorReadings.Remove(reading);
            await _uow.CompleteAsync();
            return true;
        }

        public async Task<decimal> GetAverageForHouseAsync(int houseId)
        {
            var readings = await _uow.SensorReadings
                .GetAllQueryable()
                .Where(r => r.HouseId == houseId)
                .ToListAsync();

            if (readings.Count == 0)
                return 0;

            return readings.Average(r => r.Temperature);
        }

        public async Task<int> CountReadingsBelowAsync(decimal threshold)
        {
            return await _uow.SensorReadings
                .GetAllQueryable()
                .CountAsync(r => r.Temperature <= threshold);
        }
    }
}
