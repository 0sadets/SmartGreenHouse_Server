using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;
using System.Security.Claims;

namespace SmartGreenhouse.Controllers
{
    [Authorize]
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

        
        [HttpPost("create")]
        public async Task<IActionResult> CreateGreenhouse([FromBody] GreenhouseCreateDto dto)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");


            if (userIdClaim == null)
                return Unauthorized("Користувач не авторизований.");

            int userId = int.Parse(userIdClaim.Value);

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
                var status = _greenhouseService.SaveGreenhouseStatusRecord(greenhouseId);
                // Тепер status вже містить оновлений статус, не потрібно знову викликати Evaluate
                return Ok(status);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера, {ex.Message}");
            }
        }


        [HttpGet("user-greenhouses")]
        
        public IActionResult GetUserGreenhouses()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized("ID користувача не знайдено або невалідне.");

            var result = _greenhouseService.GetGreenhousesByUserId(userId);
            return Ok(result);
        }
        [HttpGet("{greenhouseId}")]
        public ActionResult<GreenhouseReadDto> GetGreenhouseById(int greenhouseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(new { message = "Користувач не авторизований" });

            int userId = int.Parse(userIdClaim.Value);

            var greenhouse = _greenhouseService.GetById(userId, greenhouseId);

            if (greenhouse == null)
                return NotFound(new { message = "Теплиця не знайдена або не належить користувачу." });

            return Ok(greenhouse);
        }
        [HttpPost("assign-device")]
        public async Task<IActionResult> AssignDevice([FromBody] AssignDeviceDto dto)
        {
            await _greenhouseService.AssignDeviceToGreenhouseAsync(dto.SerialNumber, dto.GreenhouseId);
            return Ok();
        }
        [HttpGet("device/{serialNumber}/greenhouse-id")]
        public IActionResult GetGreenhouseIdByDevice(string serialNumber = "ARDUINO-001")
        {
            var greenhouseId = _greenhouseService.GetAssignedGreenhouseId(serialNumber);
            if (greenhouseId == null)
                return NotFound("Пристрій не знайдено або не прив’язаний до теплиці.");

            return Ok(greenhouseId);
        }

        [HttpPut("{greenhouseId}")]
        public IActionResult UpdateGreenhouse(int greenhouseId, [FromBody] GreenhouseUpdateDto dto)
        {
            try
            {
                _greenhouseService.UpdateGreenhouse( greenhouseId, dto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteGreenhouse(int id)
        {
            try
            {
                _greenhouseService.DeleteGreenhouseWithDependencies(id);
                return NoContent(); // 204 - успішно, без тіла
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


    }
}
