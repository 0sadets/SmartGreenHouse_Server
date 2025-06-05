using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartGreenhouse.Configuration;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using System.IO.Ports;

namespace SmartGreenhouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : Controller
    {
        private readonly IDeviceStateService _deviceStateService;

        public DeviceController(IDeviceStateService deviceStateService)
        {
            _deviceStateService = deviceStateService;
        }

        [HttpPost("state")]
        public async Task<IActionResult> UpdateDeviceState([FromBody] DeviceUpdateRequest request)
        {
            if (request.DeviceType != "fan" && request.DeviceType != "door")
                return BadRequest("DeviceType must be 'fan' or 'door'.");

            try
            {
                await _deviceStateService.UpdateDeviceStateAsync(request);
                return Ok("Стан оновлено та команду відправлено.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка: {ex.Message}");
            }
        }
        [HttpGet("{greenhouseId}/last-state")]
        public async Task<IActionResult> GetLastDeviceState(int greenhouseId)
        {
            var lastStateDto = await _deviceStateService.GetLastDeviceStateAsync(greenhouseId);
            if (lastStateDto == null)
                return NotFound("Стан для теплиці не знайдений.");

            return Ok(lastStateDto);
        }





    }
}
