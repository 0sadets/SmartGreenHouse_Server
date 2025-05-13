using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartGreenhouse.Interfaces;

namespace SmartGreenhouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantController : ControllerBase
    {
        private readonly IPlantService _plantService;

        public PlantController(IPlantService plantService)
        {
            _plantService = plantService;
        }

        [HttpGet("all")]
        public IActionResult GetAllPlantsWithExamples()
        {
            var plants = _plantService.GetAllPlantsWithExamples();
            return Ok(plants);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPlantById(int id)
        {
            var plant = _plantService.GetPlantById(id);
            if (plant == null) return NotFound("Plant not found");
            return Ok(plant);
        }

        [HttpGet("by-category/{category}")]
        public IActionResult GetPlantByCategory(string category)
        {
            var plant = _plantService.GetPlantByCategory(category);
            if (plant == null) return NotFound("Plant category not found");
            return Ok(plant);
        }
        [HttpGet("categories-with-examples")]
        public IActionResult GetCategoriesWithExamples()
        {
            var categoriesWithExamples = _plantService.GetPlantsExamples();
            return Ok(categoriesWithExamples);
        }

    }



}
