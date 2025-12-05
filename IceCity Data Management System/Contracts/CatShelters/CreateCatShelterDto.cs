namespace IceCity_Data_Management_System.Contracts.CatShelters;

public class CreateCatShelterDto : CreateHouseDto
{
    public int Id { get; set; }
    public string CatName { get; set; } = string.Empty;
}