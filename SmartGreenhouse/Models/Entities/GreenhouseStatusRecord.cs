namespace SmartGreenhouse.Models.Entities
{
    public class GreenhouseStatusRecord
    {
        public int Id { get; set; }

        public int GreenhouseId { get; set; }
        public Greenhouse Greenhouse { get; set; }

        public DateTime Timestamp { get; set; }

        public string Status { get; set; } 

        public string AlertsJson { get; set; }
    }

}
