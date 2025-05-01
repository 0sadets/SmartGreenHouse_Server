namespace SmartGreenhouse.Models.Entities
{
    public class Plant
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public float OptimalAirTempMin { get; set; }
        public float OptimalAirTempMax { get; set; }

        public float OptimalAirHumidityMin { get; set; }
        public float OptimalAirHumidityMax { get; set; }

        public float OptimalSoilHumidityMin { get; set; }
        public float OptimalSoilHumidityMax { get; set; }

        public float OptimalLightMin { get; set; }
        public float OptimalLightMax { get; set; }
        public ICollection<GreenhousePlant> GreenhousePlants { get; set; }

    }
}
