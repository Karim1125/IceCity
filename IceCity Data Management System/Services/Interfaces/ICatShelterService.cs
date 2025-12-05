namespace IceCity_Data_Management_System.Services.Interfaces
{
    public interface ICatShelterService
    {
        Task<List<CatShelterDto>> GetAllAsync();
        Task<CatShelterDto?> GetByIdAsync(int id);
        Task<CatShelterDto> AddAsync(CreateCatShelterDto dto);

        Task<bool> UpdateCatNameAsync(int id, string newCatName);
        Task<bool> DeleteAsync(int id);

        // qeuries
        Task<int> CountSheltersWithMoreThanXHeatersAsync(int heatersCount);
    }
}
