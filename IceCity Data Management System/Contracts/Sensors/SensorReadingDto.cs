namespace IceCity_Data_Management_System.Contracts.Sensors;

public class SensorReadingDto
{
    public int Id { get; set; }
    public int HouseId { get; set; }
    public decimal Temperature { get; set; }
    public DateTime Timestamp { get; set; }
}