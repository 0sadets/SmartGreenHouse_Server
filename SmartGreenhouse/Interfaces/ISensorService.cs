using SmartGreenhouse.Models.DTOs;

namespace SmartGreenhouse.Interfaces
{
    public interface ISensorService
    {
        GreenhouseStatusDto AddSensorReading(SensorReadingCreateDto dto);
    }
}
