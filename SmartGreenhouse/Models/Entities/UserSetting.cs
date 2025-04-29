namespace SmartGreenhouse.Models.Entities
{
    public class UserSetting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GreenhouseId { get; set; }

        public float TargetAirTemp { get; set; }
        public float TargetAirHumidity { get; set; }
        public float TargetSoilMoisture { get; set; }
        public float TargetLight { get; set; }

        public User User { get; set; }
        public Greenhouse Greenhouse { get; set; }
    }
}
