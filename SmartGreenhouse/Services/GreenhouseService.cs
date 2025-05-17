using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<Device> _deviceRepository;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IRepository<SensorReading> _sensorReadingRepository;


        public GreenhouseService(
            IRepository<Greenhouse> repository,
            IRepository<Plant> plantRepository,
            IRepository<UserSetting> userSettingRepository, IMapper mapper,
            IRepository<Device> deviceRepository,
             IUserSettingsService userSettingsService,
             IRepository<SensorReading> sensorReadingRepository
            ) 
     
        {
            _repository = repository;
            _plantRepository = plantRepository;
            _userSettingRepository = userSettingRepository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _userSettingsService = userSettingsService;
            _sensorReadingRepository = sensorReadingRepository;
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

        public async Task<Greenhouse> CreateWithOptimalSettingsAsync(GreenhouseCreateDto dto, int userId)
        {
            var selectedPlants = _plantRepository.Get(p => dto.PlantIds.Contains(p.Id)).ToList();
            if (selectedPlants.Count != dto.PlantIds.Count)
                throw new ArgumentException("Деякі обрані рослини не знайдено.");

            // 1. обчислюємо обєм теплиці
            float volume = dto.Length * dto.Width * dto.Height;

            // 2. коригувальний коефіцієнт в залежності від сезону
            float tempAdjustment = 0;
            float lightAdjustment = 0;

            switch (dto.Season.ToLower())
            {
                case "winter":
                    tempAdjustment = 2; // збільшуємо рекомендовану температуру
                    lightAdjustment = 2; // більше годин світла
                    break;
                case "summer":
                    tempAdjustment = -1; // трохи знижуємо
                    lightAdjustment = -1;
                    break;
                case "spring":
                case "autumn":
                    tempAdjustment = 0;
                    lightAdjustment = 0;
                    break;
            }

            // 3. базові діапазони з урахуванням сезонного коригування
            var settingDto = new CreateUserSettingsDto
            {
                GreenhouseId = 0, // тимчасово, оновимо пізніше
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
                LightHoursPerDay = selectedPlants.Average(p => p.OptimalLightHourPerDay) + lightAdjustment
            };

            // 4. створення теплиці
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

            Create(greenhouse);
            Save();

            settingDto.GreenhouseId = greenhouse.Id;

            var setting = _mapper.Map<UserSetting>(settingDto);
            setting.UserId = userId;

            _userSettingRepository.Create(setting);
            _userSettingRepository.Save();

            // 5.привязка аrduino
            var existingDevice = _deviceRepository.Get(d => d.SerialNumber == "ARDUINO-001").FirstOrDefault();

            if (existingDevice != null)
            {
                existingDevice.GreenhouseId = greenhouse.Id;
                _deviceRepository.Update(existingDevice);
            }
            else
            {
                var device = new Device
                {
                    SerialNumber = "ARDUINO-001",
                    GreenhouseId = greenhouse.Id
                };
                _deviceRepository.Create(device);
            }
            _deviceRepository.Save();

            // 6. Рекомендація (вивід у лог, поки не зберігаємо)
            Console.WriteLine($"Рекомендація для теплиці '{greenhouse.Name}':");
            Console.WriteLine($"- Обʼєм: {volume} м³");
            Console.WriteLine($"- Сезон: {dto.Season}");
            Console.WriteLine($"- Температура скоригована на: {tempAdjustment}°C");
            Console.WriteLine($"- Світловий день скоригований на: {lightAdjustment} годин");
            Console.WriteLine($"- LightHoursPerDay: {settingDto.LightHoursPerDay}");

            return greenhouse;
        }

        public GreenhouseRecommendationDto GetRecommendation(GreenhouseCreateDto dto, List<Plant> selectedPlants)
        {
            float volume = dto.Length * dto.Width * dto.Height;
            string season = dto.Season;

            float avgLight = selectedPlants.Average(p => p.OptimalLightHourPerDay);
            float avgAirTemp = (selectedPlants.Min(p => p.OptimalAirTempMin) + selectedPlants.Max(p => p.OptimalAirTempMax)) / 2;

            string tempAdvice = "";
            string lightAdvice = "";
            string humidityAdvice = "";
            string generalTip = "";

            switch (season.ToLower())
            {
                case "winter":
                    tempAdvice = "Рекомендується трохи підвищити температуру в теплиці через холодну пору року.";
                    lightAdvice = "Слід збільшити штучне освітлення на 1–2 години щодня.";
                    generalTip = "Використовуйте термоізоляційні матеріали, щоб зберегти тепло.";
                    break;
                case "summer":
                    tempAdvice = "У літній період важливо уникати перегріву. Можна використовувати вентиляцію.";
                    lightAdvice = "Природного світла достатньо, штучне освітлення можна мінімізувати.";
                    generalTip = "Зверніть увагу на провітрювання, щоб уникнути перегріву.";
                    break;
                case "spring":
                case "autumn":
                    tempAdvice = "Температурні умови більш стабільні, але слідкуйте за нічним похолоданням.";
                    lightAdvice = "Можливо, буде потрібно додаткове освітлення у хмарні дні.";
                    generalTip = "Сезон підходить для більшості культур, але контролюйте вологість.";
                    break;
                default:
                    generalTip = "Сезон не розпізнано. Перевірте введені дані.";
                    break;
            }

            humidityAdvice = "Підтримуйте рівень вологості відповідно до вимог вибраних культур.";

            return new GreenhouseRecommendationDto
            {
                Volume = volume,
                Season = dto.Season,
                TemperatureAdvice = tempAdvice,
                LightAdvice = lightAdvice,
                HumidityAdvice = humidityAdvice,
                GeneralTip = generalTip
            };
        }
        public GreenhouseRecommendationDto GetRecommendationByGreenhouseId(int greenhouseId)
        {
            var greenhouse = _repository.Get(g => g.Id == greenhouseId, includeProperties: "Plants").FirstOrDefault();
            if (greenhouse == null)
                throw new ArgumentException("Теплиця не знайдена.");

            var selectedPlants = greenhouse.Plants.ToList();

            var dto = new GreenhouseCreateDto
            {
                Length = greenhouse.Length,
                Width = greenhouse.Width,
                Height = greenhouse.Height,
                Season = greenhouse.Season,
                PlantIds = selectedPlants.Select(p => p.Id).ToList()
            };

            return GetRecommendation(dto, selectedPlants);
        }

        public GreenhouseStatusDto EvaluateGreenhouseStatus(int greenhouseId)
        {
            var result = new GreenhouseStatusDto();

            var reading = _sensorReadingRepository.Get(
                filter: r => r.GreenhouseId == greenhouseId,
                orderBy: q => q.OrderByDescending(r => r.Timestamp)
            ).FirstOrDefault();

            if (reading == null || string.IsNullOrWhiteSpace(reading.DeviceSerialNumber))
            {
                result.Status = "NoData";
                result.Alerts.Add("Немає даних від сенсорів.");
                return result;
            }

            var userSettings = _userSettingsService.GetByGreenhouseId(greenhouseId);
            if (userSettings == null)
            {
                result.Status = "NoData";
                result.Alerts.Add("Налаштування не знайдено.");
                return result;
            }

            int criticalCount = 0;
            int warningCount = 0;

            void Check(string name, float value, float min, float max)
            {
                if (value < min * 0.9 || value > max * 1.1)
                {
                    result.Alerts.Add($"{name}: критичне відхилення ({value})");
                    criticalCount++;
                }
                else if (value < min || value > max)
                {
                    result.Alerts.Add($"{name}: незначне відхилення ({value})");
                    warningCount++;
                }
            }

            Check("Температура повітря", reading.AirTemp, userSettings.AirTempMin, userSettings.AirTempMax);
            Check("Вологість повітря", reading.AirHum, userSettings.AirHumidityMin, userSettings.AirHumidityMax);
            Check("Вологість ґрунту", reading.SoilHum, userSettings.SoilHumidityMin, userSettings.SoilHumidityMax);
            Check("Температура ґрунту", reading.SoilTemp, userSettings.SoilTempMin, userSettings.SoilTempMax);
            Check("Освітленість", reading.LightLevel, userSettings.LightMin, userSettings.LightMax);

            result.Status = criticalCount > 0 ? "Critical"
                         : warningCount > 0 ? "Warning"
                         : "Normal";

            return result;
        }




    }
}
