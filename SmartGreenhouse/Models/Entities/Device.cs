namespace SmartGreenhouse.Models.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; } 
        public int GreenhouseId { get; set; }
        public Greenhouse Greenhouse { get; set; }
    }

}
