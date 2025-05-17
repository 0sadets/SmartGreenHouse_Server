using SmartGreenhouse.Models.DTOs;

namespace SmartGreenhouse.Interfaces
{
    public interface IUserSettingsService
    {
        UserSettingsDto GetByGreenhouseId(int greenhouseId);
    }
}
