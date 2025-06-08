using SmartGreenhouse.Models.DTOs;

namespace SmartGreenhouse.Interfaces
{
    public interface IUserService
    {
        Task<UserReadDto> GetUserInfoAsync(int userId);
        Task UpdateUserAsync(int userId, UserUpdateDto dto);
        Task ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}
