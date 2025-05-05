using Microsoft.IdentityModel.Tokens;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartGreenhouse.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public async Task<string> GenerateAccessToken(AppUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateAndStoreRefreshToken(AppUser user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.UtcNow.AddDays(7), // або 30 днів
                AppUserId = user.Id,
                IsRevoked = false
            };

            await _userRepository.CreateRefreshTokenAsync(refreshToken);
            await _userRepository.SaveAsync();

            return refreshToken.Token;
        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokens(string refreshToken)
        {
            var existingToken = await _userRepository.GetRefreshTokenAsync(refreshToken);
            if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiresAt <= DateTime.UtcNow)
                throw new SecurityTokenException("Invalid or expired refresh token");

            await _userRepository.RevokeRefreshTokenAsync(existingToken);

            var newAccessToken = await GenerateAccessToken(existingToken.AppUser);
            var newRefreshToken = await GenerateAndStoreRefreshToken(existingToken.AppUser);

            return (newAccessToken, newRefreshToken);
        }
    }
}
