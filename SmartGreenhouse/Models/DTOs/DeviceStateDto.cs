namespace SmartGreenhouse.Models.DTOs
{
    public class DeviceStateDto
    {
        public string ghId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool DoorStatus { get; set; }
        public bool FanStatus { get; set; }
    }
}
