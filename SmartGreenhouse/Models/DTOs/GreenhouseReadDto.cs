namespace SmartGreenhouse.Models.DTOs
{
    public class GreenhouseReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Season { get; set; }
        public string Location { get; set; }

        public List<PlantReadDto> Plants { get; set; }
    }


}
