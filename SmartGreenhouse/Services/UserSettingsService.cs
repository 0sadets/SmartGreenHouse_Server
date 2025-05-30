using AutoMapper;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;
using SmartGreenhouse.Repositories;
using System.Security.Claims;

namespace SmartGreenhouse.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IRepository<UserSetting> _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Greenhouse> _greenhouseRepository;

        public UserSettingsService(
            IRepository<UserSetting> repository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Greenhouse> greenhouseRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _greenhouseRepository = greenhouseRepository;
        }

        public UserSettingsDto GetByGreenhouseId(int greenhouseId)
        {
            var setting = _repository.Get(s => s.GreenhouseId == greenhouseId).FirstOrDefault();

            return setting == null ? null : _mapper.Map<UserSettingsDto>(setting);
        }
        public IEnumerable<UserSettingsDto> GetByUserAndGreenhouse( int greenhouseId)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Enumerable.Empty<UserSettingsDto>();

            int userId = int.Parse(userIdClaim.Value);

            var settings = _repository.Get(
                s => s.UserId == userId && s.GreenhouseId == greenhouseId
            );

            return _mapper.Map<IEnumerable<UserSettingsDto>>(settings);
        }
        public bool UpdateUserSettings(int greenhouseId, UpdateUserSettingsDto dto)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return false;

            int userId = int.Parse(userIdClaim.Value);

            var setting = _repository.Get(s => s.UserId == userId && s.GreenhouseId == greenhouseId).FirstOrDefault();
            if (setting == null) return false;

            // Оновлення полів
            setting.AirTempMin = dto.AirTempMin;
            setting.AirTempMax = dto.AirTempMax;
            setting.AirHumidityMin = dto.AirHumidityMin;
            setting.AirHumidityMax = dto.AirHumidityMax;
            setting.SoilHumidityMin = dto.SoilHumidityMin;
            setting.SoilHumidityMax = dto.SoilHumidityMax;
            setting.SoilTempMin = dto.SoilTempMin;
            setting.SoilTempMax = dto.SoilTempMax;
            setting.LightMin = dto.LightMin;
            setting.LightMax = dto.LightMax;

            _repository.Update(setting);
            _repository.Save();

            return true;
        }
        public CreateUserSettingsDto GenerateOptimalSettings(int greenhouseId)
        {
            var greenhouse = _greenhouseRepository.GetById(greenhouseId);
            if (greenhouse == null)
                throw new ArgumentException("Теплицю не знайдено.");

            var selectedPlants = greenhouse.Plants.ToList();
            if (!selectedPlants.Any())
                throw new InvalidOperationException("У теплиці немає рослин.");
            // 1. обчислюємо обєм теплиці
            float volume = greenhouse.Length * greenhouse.Width * greenhouse.Height;

            // 2. коригувальний коефіцієнт в залежності від сезону
            float tempAdjustment = 0;
            float lightAdjustment = 0;

            switch (greenhouse.Season.ToLower())
            {
                case "winter":
                    tempAdjustment = 2;
                    lightAdjustment = 2;
                    break;
                case "summer":
                    tempAdjustment = -1;
                    lightAdjustment = -1;
                    break;
            }

            var settingDto = new CreateUserSettingsDto
            {
                GreenhouseId = greenhouse.Id,
                AirTempMin = selectedPlants.Min(p => p.OptimalAirTempMin) + tempAdjustment,
                AirTempMax = selectedPlants.Max(p => p.OptimalAirTempMax) + tempAdjustment,
                AirHumidityMin = selectedPlants.Min(p => p.OptimalAirHumidityMin),
                AirHumidityMax = selectedPlants.Max(p => p.OptimalAirHumidityMax),
                SoilHumidityMin = selectedPlants.Min(p => p.OptimalSoilHumidityMin),
                SoilHumidityMax = selectedPlants.Max(p => p.OptimalSoilHumidityMax),
                SoilTempMin = selectedPlants.Min(p => p.OptimalSoilTempMin) + tempAdjustment,
                SoilTempMax = selectedPlants.Max(p => p.OptimalSoilTempMax) + tempAdjustment,
                LightMin = selectedPlants.Min(p => p.OptimalLightMin),
                LightMax = selectedPlants.Max(p => p.OptimalLightMax),
                //LightHoursPerDay = selectedPlants.Average(p => p.OptimalLightHourPerDay) + lightAdjustment
            };

            return settingDto;
        }

        public bool UpdateSettingsForGreenhouse(int greenhouseId, CreateUserSettingsDto dto)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return false;

            int userId = int.Parse(userIdClaim.Value);

            var setting = _repository.Get(s => s.UserId == userId && s.GreenhouseId == greenhouseId).FirstOrDefault();
            if (setting == null)
            {
                // Технічно цього не має бути, але на всяк випадок можна логувати або кидати помилку
                return false;
            }

            // Оновлення полів
            setting.AirTempMin = dto.AirTempMin;
            setting.AirTempMax = dto.AirTempMax;
            setting.AirHumidityMin = dto.AirHumidityMin;
            setting.AirHumidityMax = dto.AirHumidityMax;
            setting.SoilHumidityMin = dto.SoilHumidityMin;
            setting.SoilHumidityMax = dto.SoilHumidityMax;
            setting.SoilTempMin = dto.SoilTempMin;
            setting.SoilTempMax = dto.SoilTempMax;
            setting.LightMin = dto.LightMin;
            setting.LightMax = dto.LightMax;

            _repository.Update(setting);
            _repository.Save();

            return true;
        }


    }

}
