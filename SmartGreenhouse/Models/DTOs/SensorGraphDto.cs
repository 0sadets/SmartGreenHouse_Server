namespace SmartGreenhouse.Models.DTOs
{
    public class SensorGraphDto
    {
        public List<float?> Values { get; set; } = new();
        public float Min { get; set; }         
        public float Max { get; set; }
    }
}
