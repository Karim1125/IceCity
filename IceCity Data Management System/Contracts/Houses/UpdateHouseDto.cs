namespace IceCity_Data_Management_System.Contracts.Houses;

public class UpdateHouseDto
{
    public int Id { get; set; }
    public int Heaters { get; set; }
    public int HoursPerDay { get; set; }
    public decimal OutsideTemperature { get; set; }
}
