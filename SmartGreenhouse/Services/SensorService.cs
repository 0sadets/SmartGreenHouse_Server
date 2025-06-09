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
        private readonly IUserSettingsService _usSettService;

        public SensorService(IRepository<SensorReading> repository, 
            IMapper mapper, IRepository<Device> deviceRepository, 
            IRepository<Greenhouse> ghRepository, 
            IGreenhouseService greenhouseService,
            IUserSettingsService usSettService)
        {
            _repository = repository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _ghRepository = ghRepository;
            _greenhouseService = greenhouseService;
            _usSettService = usSettService;
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
        public async Task<Dictionary<string, SensorGraphDto>> GetAllChartDataAsync(int greenhouseId)
        {
            var yesterday = DateTime.UtcNow.Date.AddDays(-1);
            var today = yesterday.AddDays(1);

            var sensorData = await _repository.GetAsync(
                filter: s => s.GreenhouseId == greenhouseId && s.Timestamp >= yesterday && s.Timestamp < today,
                orderBy: q => q.OrderBy(x => x.Timestamp)
            );

            var greenhouse = (await _ghRepository.GetAsync(g => g.Id == greenhouseId)).FirstOrDefault();

            if (greenhouse == null) throw new Exception("Теплиця не знайдена");

            var settings = _usSettService.GetByGreenhouseId(greenhouseId);

            if (settings == null)
                throw new Exception("Не знайдено налаштування для теплиці");


            var result = new Dictionary<string, SensorGraphDto>
            {
                ["airTemp"] = new SensorGraphDto
                {
                    Values = sensorData.Select(d => (float?)d.AirTemp).ToList(),
                    Min = settings.AirTempMin,
                    Max = settings.AirTempMax
                },
                ["airHum"] = new SensorGraphDto
                {
                    Values = sensorData.Select(d => (float?)d.AirHum).ToList(),
                    Min = settings.AirHumidityMin,
                    Max = settings.AirHumidityMax
                },
                ["soilTemp"] = new SensorGraphDto
                {
                    Values = sensorData.Select(d => (float?)d.SoilTemp).ToList(),
                    Min = settings.SoilTempMin,
                    Max = settings.SoilTempMax
                },
                ["soilHum"] = new SensorGraphDto
                {
                    Values = sensorData.Select(d => (float?)d.SoilHum).ToList(),
                    Min = settings.SoilHumidityMin,
                    Max = settings.SoilHumidityMax
                },
                ["light"] = new SensorGraphDto
                {
                    Values = sensorData.Select(d => (float?)d.LightLevel).ToList(),
                    Min = settings.LightMin,
                    Max = settings.LightMax
                }
            };

            return result;
        }






    }
}
