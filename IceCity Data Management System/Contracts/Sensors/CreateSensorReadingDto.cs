namespace IceCity_Data_Management_System.Contracts.Sensors;

public class CreateSensorReadingDto
{
    public int HouseId { get; set; }
    public decimal Temperature { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}