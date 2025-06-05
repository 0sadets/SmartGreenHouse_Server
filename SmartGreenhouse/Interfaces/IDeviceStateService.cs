using SmartGreenhouse.Models.DTOs;

namespace SmartGreenhouse.Interfaces
{
    public interface IDeviceStateService
    {
        Task<bool> UpdateDeviceStateAsync(DeviceUpdateRequest request);
        Task<DeviceStateDto?> GetLastDeviceStateAsync(int greenhouseId);
        // Task SaveDeviceStateAsync(DeviceStateDto dto);
    }
}
