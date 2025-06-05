namespace SmartGreenhouse.Models.DTOs
{
    public class DeviceUpdateRequest
    {
        public string DeviceType { get; set; } 
        public bool NewState { get; set; }
    }
}
