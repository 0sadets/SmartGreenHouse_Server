namespace SmartGreenhouse.Models.Entities
{
    public class User
    {

        public ICollection<Greenhouse> Greenhouses { get; set; }
        public ICollection<UserSetting> UserSettings { get; set; }
    }
}
