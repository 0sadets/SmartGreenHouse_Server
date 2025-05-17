using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;
using System.Security.Claims;

namespace SmartGreenhouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreenhouseController : ControllerBase
    {
        private readonly IGreenhouseService _greenhouseService;

        private readonly IRepository<Plant> _plantRepository;

        public GreenhouseController(IGreenhouseService greenhouseService, IRepository<Plant> plantRepository)
        {
            _greenhouseService = greenhouseService;
            _plantRepository = plantRepository;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGreenhouse([FromBody] GreenhouseCreateDto dto)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");


            if (userIdClaim == null)
                return Unauthorized("Користувач не авторизований.");

            int userId = int.Parse(userIdClaim.Value);
            //int userId = 1;

            try
            {
                var greenhouse = await _greenhouseService.CreateWithOptimalSettingsAsync(dto, userId);
                return Ok(greenhouse); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}/recommendation")]
        public IActionResult GetRecommendation(int id)
        {
            try
            {
                var recommendation = _greenhouseService.GetRecommendationByGreenhouseId(id);
                return Ok(recommendation);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("status")]
        public IActionResult CheckGreenhouseStatus([FromBody] int greenhouseId)
        {
            try
            {
                var status = _greenhouseService.EvaluateGreenhouseStatus(greenhouseId);
                return Ok(status);

            }
            catch (ArgumentException ex) {
                return StatusCode(500, $"Внутрішня помилка сервера, {ex.Message}");
            }
        }


    }
}
