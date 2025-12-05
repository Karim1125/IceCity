using IceCity_Data_Management_System.Services.Interfaces;

namespace IceCity_Data_Management_System.Services.Implementations
{
    public class CatShelterService(IUnitOfWork uow, IMapper mapper) : ICatShelterService
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        public async Task<List<CatShelterDto>> GetAllAsync()
        {
            var shelters = await _uow.CatShelters.GetAllAsync();
            return _mapper.Map<List<CatShelterDto>>(shelters);
        }

        public async Task<CatShelterDto?> GetByIdAsync(int id)
        {
            var shelter = await _uow.CatShelters.GetByIdAsync(id);
            return shelter == null ? null : _mapper.Map<CatShelterDto>(shelter);
        }

        public async Task<CatShelterDto> AddAsync(CreateCatShelterDto dto)
        {
            var house = await _uow.Houses.GetByIdAsync(dto.Id)
                ?? throw new Exception("House not found.");

            if (house is CatShelter existing)
            {
                existing.CatName = dto.CatName;
                _uow.CatShelters.Update(existing);
                await _uow.CompleteAsync();
                return _mapper.Map<CatShelterDto>(existing);
            }

            _uow.Context.Entry(house).State = EntityState.Detached;

            var shelter = new CatShelter
            {
                Id = house.Id,
                OwnerName = house.OwnerName,
                Heaters = house.Heaters,
                HoursPerDay = house.HoursPerDay,
                OutsideTemperature = house.OutsideTemperature,
                CatName = dto.CatName
            };

            _uow.CatShelters.Update(shelter);
            await _uow.CompleteAsync();

            return _mapper.Map<CatShelterDto>(shelter);
        }

        public async Task<bool> UpdateCatNameAsync(int id, string newCatName)
        {
            var shelter = await _uow.CatShelters.GetByIdAsync(id);
            if (shelter == null)
                return false;

            shelter.CatName = newCatName;
            _uow.CatShelters.Update(shelter);
            await _uow.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var shelter = await _uow.CatShelters.GetByIdAsync(id);
            if (shelter == null)
                return false;

            _uow.CatShelters.Remove(shelter);
            await _uow.CompleteAsync();
            return true;
        }

        public async Task<int> CountSheltersWithMoreThanXHeatersAsync(int heatersCount)
        {
            return await _uow.CatShelters
                .GetAllQueryable()
                .CountAsync(s => s.Heaters > heatersCount);
        }
    }
}
