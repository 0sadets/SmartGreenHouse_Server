using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateAccessToken(AppUser user);
        Task<string> GenerateAndStoreRefreshToken(AppUser user);
        Task<(string accessToken, string refreshToken)> RefreshTokens(string refreshToken);

    }
}
