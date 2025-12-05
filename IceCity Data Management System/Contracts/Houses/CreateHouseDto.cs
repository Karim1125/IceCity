namespace IceCity_Data_Management_System.Contracts.Houses;

public class CreateHouseDto
{
    public string OwnerName { get; set; } = string.Empty;
    public int Heaters { get; set; }
    public int HoursPerDay { get; set; }
    public decimal OutsideTemperature { get; set; }
}