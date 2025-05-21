using SmartGreenhouse.Models.DTOs;

namespace SmartGreenhouse.Interfaces
{
    public interface IUserSettingsService
    {
        UserSettingsDto GetByGreenhouseId(int greenhouseId);
        IEnumerable<UserSettingsDto> GetByUserAndGreenhouse(int greenhouseId);
        bool UpdateUserSettings(int greenhouseId, UpdateUserSettingsDto dto);
        CreateUserSettingsDto GenerateOptimalSettings(int greenhouseId);



    }
}
