namespace SmartGreenhouse.Models.Entities
{
    public class DeviceState
    {
        public int Id { get; set; }
        public int GreenhouseId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool DoorStatus { get; set; }
        public bool FanStatus { get; set; }

        public Greenhouse Greenhouse { get; set; }
    }
}
