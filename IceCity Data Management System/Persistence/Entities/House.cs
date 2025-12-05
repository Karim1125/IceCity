namespace IceCity_Data_Management_System.Persistence.Entities
{
    public class House
    {
        public int Id { get; set; }
        public string OwnerName { get; set; } = string.Empty;

        public int Heaters { get; set; }
        public int HoursPerDay { get; set; }
        public decimal OutsideTemperature { get; set; }
        public ICollection<SensorReading> SensorReadings { get; set; } = [];
    }
}
