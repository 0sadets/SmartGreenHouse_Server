using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Models.DTOs
{
    public class GHStatusCreateDto
    {
        public int GreenhouseId { get; set; }
       public DateTime Timestamp { get; set; }

        public string Status { get; set; }

        public string AlertsJson { get; set; }

    }
}
