using AutoMapper;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Services
{
    public class SensorService : ISensorService
    {
        private readonly IRepository<SensorReading> _repository;
        private readonly IMapper _mapper;
        private readonly IRepository<Device> _deviceRepository;
        private readonly IRepository<Greenhouse> _ghRepository;
        private readonly IGreenhouseService _greenhouseService;

        public SensorService(IRepository<SensorReading> repository, IMapper mapper, IRepository<Device> deviceRepository, IRepository<Greenhouse> ghRepository, IGreenhouseService greenhouseService)
        {
            _repository = repository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _ghRepository = ghRepository;
            _greenhouseService = greenhouseService;
        }


        public GreenhouseStatusDto AddSensorReading(SensorReadingCreateDto dto)
        {
            var device = _deviceRepository.Get(d => d.SerialNumber == dto.DeviceSerialNumber).FirstOrDefault();
            if (device == null)
                throw new ArgumentException("Пристрій не знайдено");

            var reading = _mapper.Map<SensorReading>(dto);
            reading.GreenhouseId = device.GreenhouseId;
            reading.Timestamp = DateTime.UtcNow.ToLocalTime();

            _repository.Create(reading);
            _repository.Save();

            // Переоцінка статусу для правильного GreenhouseId
            return _greenhouseService.SaveGreenhouseStatusRecord(device.GreenhouseId);
        }
        public SensorReadDto ReadSensorDataById(int ghId)
        {
            var readings = _repository.Get(r => r.GreenhouseId == ghId)
                .OrderByDescending(r => r.Timestamp)
                .ToList();

            if (!readings.Any())
                throw new InvalidOperationException("Дані з сенсорів для цієї теплиці не знайдені.");

            var latestReading = readings.First();

            var dto = new SensorReadDto
            {
                GreenhouseId = ghId,
                AirTemp = latestReading.AirTemp,
                AirHum = latestReading.AirHum,
                SoilTemp = latestReading.SoilTemp,
                SoilHum = latestReading.SoilHum,
                LightLevel = latestReading.LightLevel
            };

            return dto;
        }




    }
}
