using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetByUserNameAsync(string userName);
        Task<AppUser> GetByIdAsync(int id);
        Task CreateRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(RefreshToken token);
        Task SaveAsync();
    }

}
