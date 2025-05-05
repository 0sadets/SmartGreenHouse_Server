using AutoMapper;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Services
{
    public class PlantService : IPlantService
    {
        private readonly IRepository<Plant> _plantRepository;
        private readonly IMapper _mapper;

        public PlantService(IRepository<Plant> plantRepository, IMapper mapper)
        {
            _plantRepository = plantRepository;
            _mapper = mapper;
        }
        public IEnumerable<PlantReadDto> GetAllPlantsWithExamples()
        {
            var plants = _plantRepository.Get();
            return _mapper.Map<IEnumerable<PlantReadDto>>(plants);
        }

        public PlantReadDto GetPlantByCategory(string category)
        {
            var plant = _plantRepository.Get(p => p.Category.ToLower() == category.ToLower()).FirstOrDefault();
            return _mapper.Map<PlantReadDto>(plant);
        }

        public PlantReadDto GetPlantById(int id)
        {
            var plant = _plantRepository.Get(p => p.Id == id).FirstOrDefault();
            return _mapper.Map<PlantReadDto>(plant);
        }
    }
}
