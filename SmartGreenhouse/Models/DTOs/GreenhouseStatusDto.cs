namespace SmartGreenhouse.Models.DTOs
{
    public class GreenhouseStatusDto
    {
        public string Status { get; set; } // Normal/Warning/Critical/NoData
        public List<string> Alerts { get; set; } = new List<string>();
    }
}
