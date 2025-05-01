namespace SmartGreenhouse.Models.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }

}
