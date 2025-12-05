namespace IceCity_Data_Management_System.Persistence.Entities
{
    public class SensorReading
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public decimal Temperature { get; set; }
        public DateTime Timestamp { get; set; }
        public House? House { get; set; }
    }
}
