using AutoMapper;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Services
{
    public class GreenhouseService : IGreenhouseService
    {
        private readonly IRepository<Greenhouse> _repository;
        private readonly IRepository<Plant> _plantRepository;
        private readonly IRepository<UserSetting> _userSettingRepository;
        private readonly IMapper _mapper;
        public GreenhouseService(
            IRepository<Greenhouse> repository,
            IRepository<Plant> plantRepository,
            IRepository<UserSetting> userSettingRepository, IMapper mapper) 
     
        {
            _repository = repository;
            _plantRepository = plantRepository;
            _userSettingRepository = userSettingRepository;
            _mapper = mapper;
        }

        public IEnumerable<Greenhouse> GetAll()
        {
            return _repository.Get(includeProperties: "Plants,UserSettings");
        }

        public Greenhouse GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Create(Greenhouse entity)
        {
            _repository.Create(entity);
        }

        public void Update(Greenhouse entity)
        {
            _repository.Update(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public void Save()
        {
            _repository.Save();
        }

        //public async Task<Greenhouse> CreateWithOptimalSettingsAsync(GreenhouseCreateDto dto, int userId)
        //{
        //    var selectedPlants = _plantRepository.Get(p => dto.PlantIds.Contains(p.Id)).ToList();

        //    if (selectedPlants.Count != dto.PlantIds.Count)
        //        throw new ArgumentException("Деякі обрані рослини не знайдено.");

        //    var greenhouse = new Greenhouse
        //    {
        //        Name = dto.Name,
        //        Length = dto.Length,
        //        Width = dto.Width,
        //        Height = dto.Height,
        //        Season = dto.Season,
        //        Location = dto.Location,
        //        UserId = userId,
        //        Plants = selectedPlants
        //    };

        //    _repository.Create(greenhouse);
        //    _repository.Save();

        //    var settingDto = new CreateUserSettingsDto
        //    {
        //        GreenhouseId = greenhouse.Id,
        //        AirTempMin = selectedPlants.Max(p => p.OptimalAirTempMin),
        //        AirTempMax = selectedPlants.Min(p => p.OptimalAirTempMax),
        //        AirHumidityMin = selectedPlants.Max(p => p.OptimalAirHumidityMin),
        //        AirHumidityMax = selectedPlants.Min(p => p.OptimalAirHumidityMax),
        //        SoilHumidityMin = selectedPlants.Max(p => p.OptimalSoilHumidityMin),
        //        SoilHumidityMax = selectedPlants.Min(p => p.OptimalSoilHumidityMax),
        //        SoilTempMin = selectedPlants.Max(p => p.OptimalSoilTempMin),
        //        SoilTempMax = selectedPlants.Min(p => p.OptimalSoilTempMax),
        //        LightMin = selectedPlants.Max(p => p.OptimalLightMin),
        //        LightMax = selectedPlants.Min(p => p.OptimalLightMax),
        //        LightHoursPerDay = selectedPlants.Average(p => p.OptimalLightHourPerDay)
        //    };

        //    var setting = _mapper.Map<UserSetting>(settingDto);
        //    setting.UserId = userId;


        //    _userSettingRepository.Create(setting);
        //    _userSettingRepository.Save();

        //    return greenhouse;
        //}
        public async Task<Greenhouse> CreateWithOptimalSettingsAsync(GreenhouseCreateDto dto, int userId)
        {
            var selectedPlants = _plantRepository.Get(p => dto.PlantIds.Contains(p.Id)).ToList();

            if (selectedPlants.Count != dto.PlantIds.Count)
                throw new ArgumentException("Деякі обрані рослини не знайдено.");

            var greenhouse = new Greenhouse
            {
                Name = dto.Name,
                Length = dto.Length,
                Width = dto.Width,
                Height = dto.Height,
                Season = dto.Season,
                Location = dto.Location,
                UserId = userId,
                Plants = selectedPlants
            };

            _repository.Create(greenhouse);
            _repository.Save();

            var settingDto = new CreateUserSettingsDto
            {
                GreenhouseId = greenhouse.Id,

                AirTempMin = selectedPlants.Min(p => p.OptimalAirTempMin),
                AirTempMax = selectedPlants.Max(p => p.OptimalAirTempMax),

                AirHumidityMin = selectedPlants.Min(p => p.OptimalAirHumidityMin),
                AirHumidityMax = selectedPlants.Max(p => p.OptimalAirHumidityMax),

                SoilHumidityMin = selectedPlants.Min(p => p.OptimalSoilHumidityMin),
                SoilHumidityMax = selectedPlants.Max(p => p.OptimalSoilHumidityMax),

                SoilTempMin = selectedPlants.Min(p => p.OptimalSoilTempMin),
                SoilTempMax = selectedPlants.Max(p => p.OptimalSoilTempMax),

                LightMin = selectedPlants.Min(p => p.OptimalLightMin),
                LightMax = selectedPlants.Max(p => p.OptimalLightMax),
                LightHoursPerDay = selectedPlants.Average(p => p.OptimalLightHourPerDay)
            };

            var setting = _mapper.Map<UserSetting>(settingDto);
            setting.UserId = userId;

            _userSettingRepository.Create(setting);
            _userSettingRepository.Save();

            return greenhouse;
        }

    }
}
