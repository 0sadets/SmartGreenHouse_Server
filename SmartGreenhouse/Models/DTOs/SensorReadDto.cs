namespace SmartGreenhouse.Models.DTOs
{
    public class SensorReadDto
    {
        public int GreenhouseId { get; set; }
        public float AirTemp { get; set; }
        public float AirHum { get; set; }
        public float SoilHum { get; set; }
        public float SoilTemp { get; set; }
        public float LightLevel { get; set; }
    }
}
