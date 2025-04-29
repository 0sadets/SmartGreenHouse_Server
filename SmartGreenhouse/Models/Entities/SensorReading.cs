namespace SmartGreenhouse.Models.Entities
{
    public class SensorReading
    {
        public int Id { get; set; }
        public int GreenhouseId { get; set; }
        public DateTime Timestamp { get; set; }
        public float AirTemp { get; set; }
        public float AirHum { get; set; }
        public float SoilHum { get; set; }
        public float SoilTemp { get; set; }
        public float LightLevel { get; set; }

        public Greenhouse Greenhouse { get; set; }
    }
}
