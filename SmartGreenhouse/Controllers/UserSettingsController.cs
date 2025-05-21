using Microsoft.AspNetCore.Mvc;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Services;

namespace SmartGreenhouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSettingsController : Controller
    {
       
        private readonly IUserSettingsService _userSettingsService;

        public UserSettingsController(IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;
        }

        [HttpGet("{greenhouseId}")]
        public IActionResult GetByGreenhouse(int greenhouseId)
        {
            var settings = _userSettingsService.GetByUserAndGreenhouse(greenhouseId);

            if (!settings.Any())
                return NotFound("Settings not found for the specified greenhouse.");

            return Ok(settings);
        }
        [HttpPut("{greenhouseId}")]
        public IActionResult UpdateSettings(int greenhouseId, [FromBody] UpdateUserSettingsDto dto)
        {
            var result = _userSettingsService.UpdateUserSettings(greenhouseId, dto);

            if (!result)
                return NotFound("Settings not found or access denied.");

            return Ok("Settings updated successfully.");
        }
        [HttpGet("{greenhouseId}/generate-settings")]
        public IActionResult GenerateSettings(int greenhouseId)
        {
            try
            {
                var result = _userSettingsService.GenerateOptimalSettings(greenhouseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
