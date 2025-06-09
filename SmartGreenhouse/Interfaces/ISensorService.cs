using SmartGreenhouse.Models.DTOs;

namespace SmartGreenhouse.Interfaces
{
    public interface ISensorService
    {
        GreenhouseStatusDto AddSensorReading(SensorReadingCreateDto dto);
        SensorReadDto ReadSensorDataById(int ghId);
        Task<Dictionary<string, SensorGraphDto>> GetAllChartDataAsync(int greenhouseId);
    }
}
