namespace SmartGreenhouse.Models.DTOs
{
    public class CreateUserSettingsDto
    {
        public int GreenhouseId { get; set; }

        public float AirTempMin { get; set; }
        public float AirTempMax { get; set; }

        public float AirHumidityMin { get; set; }
        public float AirHumidityMax { get; set; }

        public float SoilHumidityMin { get; set; }
        public float SoilHumidityMax { get; set; }

        public float SoilTempMin { get; set; }
        public float SoilTempMax { get; set; }

        public float LightMin { get; set; }
        public float LightMax { get; set; }

        //public float LightHoursPerDay { get; set; }
    }
}
