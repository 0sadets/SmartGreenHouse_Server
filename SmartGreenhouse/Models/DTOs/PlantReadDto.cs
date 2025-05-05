namespace SmartGreenhouse.Models.DTOs
{
    public class PlantReadDto
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public float OptimalAirTempMin { get; set; }
        public float OptimalAirTempMax { get; set; }

        public float OptimalAirHumidityMin { get; set; }
        public float OptimalAirHumidityMax { get; set; }

        public float OptimalSoilHumidityMin { get; set; }
        public float OptimalSoilHumidityMax { get; set; }

        public float OptimalSoilTempMax { get; set; }
        public float OptimalSoilTempMin { get; set; }

        public float OptimalLightMin { get; set; }
        public float OptimalLightMax { get; set; }

        public float OptimalLightHourPerDay { get; set; }

        public string ExampleNames { get; set; }
        public string Features { get; set; }
    }


}
