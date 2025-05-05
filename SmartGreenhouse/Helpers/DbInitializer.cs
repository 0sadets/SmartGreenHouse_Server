using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Models
{
    public static class DbInitializer
    {
        public static void SeedPlants(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plant>().HasData(
                new Plant[]
                {
                    new Plant
                    {
                        Id = 1,
                        Category = "Овочі",
                        ExampleNames = "Помідори, Огірки, Перець, Баклажани",
                        OptimalAirTempMin = 18,
                        OptimalAirTempMax = 26,
                        OptimalAirHumidityMin = 60,
                        OptimalAirHumidityMax = 80,
                        OptimalSoilHumidityMin = 65,
                        OptimalSoilHumidityMax = 75,
                        OptimalSoilTempMin = 18,
                        OptimalSoilTempMax = 24,
                        OptimalLightMin = 18000,
                        OptimalLightMax = 30000,
                        OptimalLightHourPerDay = 10,
                        Features = "Огірки потребують більшої вологості, баклажани — стабільного тепла, перець — чутливий до перепадів температур."
                    },
                    new Plant
                    {
                        Id = 2,
                        Category = "Зелень",
                        ExampleNames = "Салат, Шпинат, Кріп, Петрушка, Базилік",
                        OptimalAirTempMin = 15,
                        OptimalAirTempMax = 22,
                        OptimalAirHumidityMin = 70,
                        OptimalAirHumidityMax = 90,
                        OptimalSoilHumidityMin = 60,
                        OptimalSoilHumidityMax = 70,
                        OptimalSoilTempMin = 16,
                        OptimalSoilTempMax = 20,
                        OptimalLightMin = 10000,
                        OptimalLightMax = 20000,
                        OptimalLightHourPerDay = 8,
                        Features = "Базилік полюбляє тепліші умови, ніж інша зелень."
                    },
                    new Plant
                    {
                        Id = 3,
                        Category = "Коренеплоди",
                        ExampleNames = "Морква, Буряк, Редис, Картопля",
                        OptimalAirTempMin = 12,
                        OptimalAirTempMax = 20,
                        OptimalAirHumidityMin = 60,
                        OptimalAirHumidityMax = 75,
                        OptimalSoilHumidityMin = 65,
                        OptimalSoilHumidityMax = 80,
                        OptimalSoilTempMin = 10,
                        OptimalSoilTempMax = 18,
                        OptimalLightMin = 15000,
                        OptimalLightMax = 25000,
                        OptimalLightHourPerDay = 9,
                        Features = "Редис швидко росте в прохолодних умовах, картопля потребує глибокого ґрунту."
                    },
                    new Plant
                    {
                        Id = 4,
                        Category = "Ягоди",
                        ExampleNames = "Полуниця, Малина, Лохина, Смородина",
                        OptimalAirTempMin = 16,
                        OptimalAirTempMax = 24,
                        OptimalAirHumidityMin = 60,
                        OptimalAirHumidityMax = 85,
                        OptimalSoilHumidityMin = 70,
                        OptimalSoilHumidityMax = 80,
                        OptimalSoilTempMin = 15,
                        OptimalSoilTempMax = 20,
                        OptimalLightMin = 20000,
                        OptimalLightMax = 30000,
                        OptimalLightHourPerDay = 8,
                        Features = "Лохина потребує кислий ґрунт; малина не переносить застійної води."
                    },
                    new Plant
                    {
                        Id = 5,
                        Category = "Фрукти",
                        ExampleNames = "Лимон, Апельсин, Гранат, Інжир",
                        OptimalAirTempMin = 20,
                        OptimalAirTempMax = 28,
                        OptimalAirHumidityMin = 50,
                        OptimalAirHumidityMax = 70,
                        OptimalSoilHumidityMin = 60,
                        OptimalSoilHumidityMax = 75,
                        OptimalSoilTempMin = 18,
                        OptimalSoilTempMax = 22,
                        OptimalLightMin = 25000,
                        OptimalLightMax = 35000,
                        OptimalLightHourPerDay = 10,
                        Features = "Цитрусові не переносять переохолодження; інжир потребує періоду спокою взимку."
                    },
                    new Plant
                    {
                        Id = 6,
                        Category = "Тепличні квіти",
                        ExampleNames = "Троянди, Орхідеї, Тюльпани",
                        OptimalAirTempMin = 18,
                        OptimalAirTempMax = 25,
                        OptimalAirHumidityMin = 60,
                        OptimalAirHumidityMax = 80,
                        OptimalSoilHumidityMin = 55,
                        OptimalSoilHumidityMax = 70,
                        OptimalSoilTempMin = 16,
                        OptimalSoilTempMax = 22,
                        OptimalLightMin = 12000,
                        OptimalLightMax = 25000,
                        OptimalLightHourPerDay = 8,
                        Features = "Орхідеї потребують розсіяного світла; тюльпани люблять прохолодні ночі."
                    },
                    new Plant
                    {
                        Id = 7,
                        Category = "Гриби",
                        ExampleNames = "Печериці, Гливи, Трюфелі",
                        OptimalAirTempMin = 14,
                        OptimalAirTempMax = 20,
                        OptimalAirHumidityMin = 80,
                        OptimalAirHumidityMax = 95,
                        OptimalSoilHumidityMin = 75,
                        OptimalSoilHumidityMax = 90,
                        OptimalSoilTempMin = 14,
                        OptimalSoilTempMax = 18,
                        OptimalLightMin = 1000,
                        OptimalLightMax = 5000,
                        OptimalLightHourPerDay = 2,
                        Features = "Гриби не потребують багато світла; трюфелі ростуть у симбіозі з деревами."
                    }
                }
            );
        }
    }
}
