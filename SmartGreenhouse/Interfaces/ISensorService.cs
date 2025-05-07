using SmartGreenhouse.Models.DTOs;

namespace SmartGreenhouse.Interfaces
{
    public interface ISensorService
    {
        void AddSensorReading(SensorReadingCreateDto dto);
    }
}
