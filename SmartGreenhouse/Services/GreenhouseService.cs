﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private readonly IRepository<GreenhouseStatusRecord> _statusRecordRepository;
        private readonly IRepository<DeviceState> _deviceStateRepository;

        public GreenhouseService(
            IRepository<Greenhouse> repository,
            IRepository<Plant> plantRepository,
            IRepository<UserSetting> userSettingRepository, IMapper mapper,
            IRepository<Device> deviceRepository,
             IUserSettingsService userSettingsService,
             IRepository<SensorReading> sensorReadingRepository,
             IRepository<GreenhouseStatusRecord> statusRecordRepository,
             IRepository<DeviceState> deviceStateRepository
            ) 
     
        {
            _repository = repository;
            _plantRepository = plantRepository;
            _userSettingRepository = userSettingRepository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _userSettingsService = userSettingsService;
            _sensorReadingRepository = sensorReadingRepository;
            _statusRecordRepository = statusRecordRepository;
            _deviceStateRepository = deviceStateRepository;
        }

        public IEnumerable<Greenhouse> GetAll()
        {
            return _repository.Get(includeProperties: "Plants,UserSettings");
        }

        public GreenhouseReadDto? GetById(int userId, int greenhouseId)
        {
            var greenhouse = _repository.Get(
                filter: g => g.UserId == userId && g.Id == greenhouseId,
                includeProperties: "Plants"
            ).FirstOrDefault();

            if (greenhouse == null)
                return null;

            return new GreenhouseReadDto
            {
                Id = greenhouse.Id,
                Name = greenhouse.Name,
                Length = greenhouse.Length,
                Width = greenhouse.Width,
                Height = greenhouse.Height,
                Season = greenhouse.Season,
                Location = greenhouse.Location,
                Plants = greenhouse.Plants.Select(p => new PlantReadDto
                {
                    Id = p.Id,
                    Category = p.Category,
                    OptimalAirTempMin = p.OptimalAirTempMin,
                    OptimalAirTempMax = p.OptimalAirTempMax,
                    OptimalAirHumidityMin = p.OptimalAirHumidityMin,
                    OptimalAirHumidityMax = p.OptimalAirHumidityMax,
                    OptimalSoilHumidityMin = p.OptimalSoilHumidityMin,
                    OptimalSoilHumidityMax = p.OptimalSoilHumidityMax,
                    OptimalSoilTempMax = p.OptimalSoilTempMax,
                    OptimalSoilTempMin = p.OptimalSoilTempMin,
                    OptimalLightMin = p.OptimalLightMin,
                    OptimalLightMax = p.OptimalLightMax,
                    OptimalLightHourPerDay = p.OptimalLightHourPerDay,
                    ExampleNames = p.ExampleNames,
                    Features = p.Features
                }).ToList()
            };

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

            var settingDto = _userSettingsService.GenerateOptimalSettings(greenhouse.Id);

            var setting = _mapper.Map<UserSetting>(settingDto);
            setting.UserId = userId;

            _userSettingRepository.Create(setting);
            _userSettingRepository.Save();

            // привязка аrduino
            await AssignDeviceToGreenhouseAsync("ARDUINO-001", greenhouse.Id);
            // ств початкових  станів
            var initialDeviceState = new DeviceState
            {
                GreenhouseId = greenhouse.Id,
                Timestamp = DateTime.UtcNow,
                FanStatus = false,
                DoorStatus = false
            };

            _deviceStateRepository.Create(initialDeviceState);
            _deviceStateRepository.Save();
            return greenhouse;
        }

        public async Task AssignDeviceToGreenhouseAsync(string serialNumber, int greenhouseId)
        {
            var greenhouse = _repository.GetById(greenhouseId);
            if (greenhouse == null)
                throw new ArgumentException("Теплицю не знайдено.");

            var device = _deviceRepository.Get(d => d.SerialNumber == serialNumber).FirstOrDefault();

            if (device != null)
            {
                device.GreenhouseId = greenhouseId;
                _deviceRepository.Update(device);
            }
            else
            {
                device = new Device
                {
                    SerialNumber = serialNumber,
                    GreenhouseId = greenhouseId
                };
                _deviceRepository.Create(device);
            }

            _deviceRepository.Save();
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
                result.Status = "nodata";
                result.Alerts.Add("Немає даних від сенсорів.");
                return result;
            }

            var userSettings = _userSettingsService.GetByGreenhouseId(greenhouseId);
            if (userSettings == null)
            {
                result.Status = "nodata";
                result.Alerts.Add("Налаштування не знайдено.");
                return result;
            }

            var greenhouse = _repository.GetById(greenhouseId);
            if (greenhouse == null)
            {
                result.Status = "nodata";
                result.Alerts.Add("Теплицю не знайдено.");
                return result;
            }

            string season = greenhouse.Season ?? "unknown";

            int criticalCount = 0;
            int warningCount = 0;

            void EvaluateParameter(string name, float value, float min, float max)
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

            float GetSeasonalLightFactor(string season)
            {
                return season.ToLower() switch
                {
                    "spring" => 1.0f,
                    "summer" => 1.2f,
                    "autumn" => 0.9f,
                    "winter" => 0.7f,
                    _ => 1.0f
                };
            }

            EvaluateParameter("Температура повітря", reading.AirTemp, userSettings.AirTempMin, userSettings.AirTempMax);
            EvaluateParameter("Вологість повітря", reading.AirHum, userSettings.AirHumidityMin, userSettings.AirHumidityMax);
            EvaluateParameter("Вологість ґрунту", reading.SoilHum, userSettings.SoilHumidityMin, userSettings.SoilHumidityMax);
            EvaluateParameter("Температура ґрунту", reading.SoilTemp, userSettings.SoilTempMin, userSettings.SoilTempMax);

            var hour = reading.Timestamp.Hour;

            if (!TryEvaluateLight(hour, reading.LightLevel, userSettings.LightMin, userSettings.LightMax, season))
            {
                result.Alerts.Add("Освітленість не перевірялась (нічний час).");
            }

            result.Status = criticalCount > 0 ? "error"
                         : warningCount > 0 ? "warning"
                         : "good";

            return result;
        }
        private bool TryEvaluateLight(int hour, float lightLevel, float lightMin, float lightMax, string season)
        {
            bool isDay = hour >= 6 && hour <= 20;

            if (!isDay)
            {
                return false;
            }

            float seasonalFactor = season.ToLower() switch
            {
                "spring" => 1.0f,
                "summer" => 1.2f,
                "autumn" => 0.9f,
                "winter" => 0.7f,
                _ => 1.0f
            };

            float expectedMin = lightMin * seasonalFactor;
            float expectedMax = lightMax * seasonalFactor;

            if (lightLevel < expectedMin * 0.9 || lightLevel > expectedMax * 1.1)
            {
                return false; 
            }
            else if (lightLevel < expectedMin || lightLevel > expectedMax)
            {
                return true;
            }

            return true;
        }


        public int? GetAssignedGreenhouseId(string serialNumber)
        {
            var device = _deviceRepository
                .Get(d => d.SerialNumber == serialNumber)
                .FirstOrDefault();

            return device?.GreenhouseId;
        }


        public GreenhouseStatusDto SaveGreenhouseStatusRecord(int greenhouseId)
        {
            var statusDto = EvaluateGreenhouseStatus(greenhouseId);

            var newRecord = new GreenhouseStatusRecord
            {
                GreenhouseId = greenhouseId,
                Timestamp = DateTime.UtcNow,
                Status = statusDto.Status,
                AlertsJson = JsonConvert.SerializeObject(statusDto.Alerts)
            };


            _statusRecordRepository.Create(newRecord);
            _statusRecordRepository.Save();
            return statusDto;
        }



        public IEnumerable<GreenhouseReadDto> GetGreenhousesByUserId(int userId)
        {
            var greenhouses = _repository.Get(
               filter: g => g.UserId == userId,
               includeProperties: "Plants" 
            );
            var result = greenhouses.Select(g => new GreenhouseReadDto
            {
                Id = g.Id,
                Name = g.Name,
                Length = g.Length,
                Width = g.Width,
                Height = g.Height,
                Season = g.Season,
                Location = g.Location,
                Plants = g.Plants.Select(p => new PlantReadDto
                {
                    Id = p.Id,
                    Category = p.Category,
                    OptimalAirTempMin = p.OptimalAirTempMin,
                    OptimalAirTempMax = p.OptimalAirTempMax,
                    OptimalAirHumidityMin = p.OptimalAirHumidityMin,
                    OptimalAirHumidityMax = p.OptimalAirHumidityMax,
                    OptimalSoilHumidityMin = p.OptimalSoilHumidityMin,
                    OptimalSoilHumidityMax = p.OptimalSoilHumidityMax,
                    OptimalSoilTempMax = p.OptimalSoilTempMax,
                    OptimalSoilTempMin = p.OptimalSoilTempMin,
                    OptimalLightMin = p.OptimalLightMin,
                    OptimalLightMax = p.OptimalLightMax,
                    OptimalLightHourPerDay = p.OptimalLightHourPerDay,
                    ExampleNames = p.ExampleNames,
                    Features = p.Features
                }).ToList()
            });

            return result;
        }
      
        public void UpdateGreenhouse( int greenhouseId, GreenhouseUpdateDto dto)
        {
            var greenhouse = _repository.Get(
                filter: g => g.Id == greenhouseId,
                includeProperties: new[] { "Plants", "UserSettings" }
            ).FirstOrDefault();


            if (greenhouse == null)
                throw new ArgumentException("Теплицю не знайдено або вона не належить цьому користувачу.");

            var oldSeason = greenhouse.Season;
            var oldDimensions = (greenhouse.Length, greenhouse.Width, greenhouse.Height);

            _mapper.Map(dto, greenhouse);

            var newPlants = _plantRepository
                .Get(filter: p => dto.PlantIds.Contains(p.Id)).ToList();

            greenhouse.Plants = newPlants;

            _repository.Update(greenhouse);
            _repository.Save();

            bool shouldRecalculate =
                oldSeason != dto.Season ||
                oldDimensions != (dto.Length, dto.Width, dto.Height) ||
                dto.PlantIds.Any();

            if (shouldRecalculate)
            {
                var newSettingsDto = _userSettingsService.GenerateOptimalSettings(greenhouseId);

                _userSettingsService.UpdateSettingsForGreenhouse(greenhouseId, newSettingsDto);
            }
        }
        public void DeleteGreenhouseWithDependencies(int greenhouseId)
        {
            var greenhouse = _repository.Get(
                filter: g => g.Id == greenhouseId,
                includeProperties: "SensorReadings,DeviceStates,UserSettings,Plants"
            ).FirstOrDefault();

            if (greenhouse == null)
                throw new Exception("Теплиця не знайдена.");

            if (greenhouse.Plants != null)
                greenhouse.Plants.Clear();

            if (greenhouse.SensorReadings != null)
            {
                foreach (var sensor in greenhouse.SensorReadings.ToList())
                {
                    _sensorReadingRepository.Delete(sensor);
                }
            }

            if (greenhouse.DeviceStates != null)
            {
                foreach (var deviceState in greenhouse.DeviceStates.ToList())
                {
                    _deviceStateRepository.Delete(deviceState);
                }
            }

            if (greenhouse.UserSettings != null)
            {
                foreach (var userSetting in greenhouse.UserSettings.ToList())
                {
                    _userSettingRepository.Delete(userSetting);
                }
            }

            _repository.Delete(greenhouse);

            _repository.Save();
        }



    }
}
