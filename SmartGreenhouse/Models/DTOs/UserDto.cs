using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; } 
        public string UserName { get; set; }
        public string Email { get; set; } 
        public ICollection<Greenhouse> Greenhouses { get; set; } 
        public ICollection<UserSetting> UserSettings { get; set; } 

    }
}
