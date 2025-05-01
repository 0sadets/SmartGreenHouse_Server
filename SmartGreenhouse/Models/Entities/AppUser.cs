using Microsoft.AspNetCore.Identity;

namespace SmartGreenhouse.Models.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public ICollection<Greenhouse> Greenhouses { get; set; }
        public ICollection<UserSetting> UserSettings { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }

    }
}
