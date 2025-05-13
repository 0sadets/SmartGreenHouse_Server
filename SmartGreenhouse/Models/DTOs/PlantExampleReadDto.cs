namespace SmartGreenhouse.Models.DTOs
{
    public class PlantExampleReadDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public List<string> ExampleNames { get; set; }
    }
}
