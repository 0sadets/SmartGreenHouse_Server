using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<AppUser> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task CreateRefreshTokenAsync(RefreshToken token)
        {
            var user = await _context.Users.FindAsync(token.AppUserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            await _context.RefreshTokens.AddAsync(token);
        }


        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .Include(t => t.AppUser)
                .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow);

            if (refreshToken == null)
            {
                throw new SecurityTokenException("Refresh token not found or expired.");
            }

            return refreshToken;
        }


        public async Task RevokeRefreshTokenAsync(RefreshToken token)
        {
            token.IsRevoked = true;
            _context.RefreshTokens.Update(token);
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving changes to the database.", ex);
            }
        }

    }
}
