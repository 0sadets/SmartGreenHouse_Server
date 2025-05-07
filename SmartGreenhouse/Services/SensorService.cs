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

        public SensorService(IRepository<SensorReading> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void AddSensorReading(SensorReadingCreateDto dto)
        {
            var reading = _mapper.Map<SensorReading>(dto);

            _repository.Create(reading);
            _repository.Save();
        }
    }
}
