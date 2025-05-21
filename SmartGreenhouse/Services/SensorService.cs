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



    }
}
