using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Interfaces
{
    public interface IUserSettingsService
    {
        UserSettingsDto GetByGreenhouseId(int greenhouseId);
        IEnumerable<UserSettingsDto> GetByUserAndGreenhouse(int greenhouseId);
        bool UpdateUserSettings(int greenhouseId, UpdateUserSettingsDto dto);
        CreateUserSettingsDto GenerateOptimalSettings(int greenhouseId);

        bool UpdateSettingsForGreenhouse(int greenhouseId, CreateUserSettingsDto dto);

        UserSetting GenerateAndSaveSettings(int greenhouseId);
    }
}
