using IceCity_Data_Management_System.Services.Interfaces;

namespace IceCity_Data_Management_System.Services.Implementations
{
    public class HouseService(IUnitOfWork uow, IMapper mapper) : IHouseService
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        public async Task<List<HouseDto>> GetAllAsync()
        {
            var houses = await _uow.Houses.GetAllAsync();
            return _mapper.Map<List<HouseDto>>(houses);
        }

        public async Task<HouseDto?> GetByIdAsync(int id)
        {
            var house = await _uow.Houses.GetByIdAsync(id);
            return house == null ? null : _mapper.Map<HouseDto>(house);
        }

        public async Task<HouseDto> AddAsync(CreateHouseDto dto)
        {
            var house = _mapper.Map<House>(dto);

            await _uow.Houses.AddAsync(house);
            await _uow.CompleteAsync();

            return _mapper.Map<HouseDto>(house);
        }

        public async Task<bool> UpdateAsync(UpdateHouseDto dto)
        {
            var house = await _uow.Houses.GetByIdAsync(dto.Id);
            if (house == null)
                return false;

            _mapper.Map(dto, house);

            _uow.Houses.Update(house);
            await _uow.CompleteAsync();

            return true;
        }

        public async Task<bool> UpdateTemperatureFromSensorsAsync(int houseId, int lastNReadings = 5)
        {
            var house = await _uow.Houses.GetByIdAsync(houseId);
            if (house == null)
                return false;

            var readings = await _uow.SensorReadings
                .GetAllQueryable()
                .Where(r => r.HouseId == houseId)
                .OrderByDescending(r => r.Timestamp)
                .Take(lastNReadings)
                .ToListAsync();

            if (readings.Count == 0)
                return false;

            var newTemp = readings.Average(r => r.Temperature);

            house.OutsideTemperature = newTemp;

            _uow.Houses.Update(house);
            await _uow.CompleteAsync();

            return true;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var house = await _uow.Houses.GetByIdAsync(id);
            if (house == null)
                return false;

            _uow.Houses.Remove(house);
            await _uow.CompleteAsync();

            return true;
        }

        // queries

        public async Task<decimal> GetAverageOutsideTemperatureAsync()
        {
            var houses = await _uow.Houses.GetAllQueryable().ToListAsync();
            return houses.Average(h => (decimal)h.OutsideTemperature);
        }

        public async Task<List<HouseDto>> GetHousesWithHeatersAboveAsync(int threshold)
        {
            var houses = await _uow.Houses
                .GetAllQueryable()
                .Where(h => h.Heaters > threshold)
                .ToListAsync();

            return _mapper.Map<List<HouseDto>>(houses);
        }

        public async Task<List<HouseDto>> GetTop3ColdestHousesAsync(DateTime from, DateTime to)
        {
            var query = _uow.Houses.GetAllQueryable()
                .Join(
                    _uow.SensorReadings.GetAllQueryable().Where(s => s.Timestamp >= from && s.Timestamp <= to),
                    h => h.Id,
                    s => s.HouseId,
                    (h, s) => new { House = h, Sensor = s }
                )
                .GroupBy(x => x.House)
                .Select(g => new { House = g.Key, AvgTemp = g.Average(x => x.Sensor.Temperature) })
                .OrderBy(x => x.AvgTemp)
                .Take(3);

            var result = await query.ToListAsync();

            return [.. result.Select(r => _mapper.Map<HouseDto>(r.House))];
        }

        public async Task<int> CountCriticalTemperatureReadingsAsync(decimal criticalThreshold)
        {
            return await _uow.SensorReadings
                .GetAllQueryable()
                .CountAsync(s => s.Temperature <= criticalThreshold);
        }
    }
}
