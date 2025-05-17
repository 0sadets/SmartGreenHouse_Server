using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Interfaces
{
    public interface IGreenhouseService
    {
        IEnumerable<Greenhouse> GetAll();
        Greenhouse GetById(int id);
        void Create(Greenhouse entity);
        void Update(Greenhouse entity);
        void Delete(int id);
        void Save();

        Task<Greenhouse> CreateWithOptimalSettingsAsync(GreenhouseCreateDto dto, int userId);
        GreenhouseRecommendationDto GetRecommendation(GreenhouseCreateDto dto, List<Plant> selectedPlants);
        GreenhouseRecommendationDto GetRecommendationByGreenhouseId(int greenhouseId);
        GreenhouseStatusDto EvaluateGreenhouseStatus(int greenhouseId);



    }
}
