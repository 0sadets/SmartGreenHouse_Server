using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Interfaces
{
    public interface IPlantService
    {
        IEnumerable<PlantReadDto> GetAllPlantsWithExamples();
        PlantReadDto GetPlantById(int id);
        PlantReadDto GetPlantByCategory(string category);
        IEnumerable<PlantExampleReadDto> GetPlantsExamples();
    }

}
