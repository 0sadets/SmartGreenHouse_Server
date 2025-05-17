namespace SmartGreenhouse.Models.DTOs
{
    public class GreenhouseRecommendationDto
    {
        public float Volume { get; set; }
        public string Season { get; set; }
        public string TemperatureAdvice { get; set; }
        public string LightAdvice { get; set; }
        public string HumidityAdvice { get; set; }
        public string GeneralTip { get; set; }
    }

}
