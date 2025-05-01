namespace SmartGreenhouse.Models.Entities
{
    public class GreenhousePlant
    {
        public int GreenhouseId { get; set; }
        public Greenhouse Greenhouse { get; set; }

        public int PlantId { get; set; }
        public Plant Plant { get; set; }
    }
}
