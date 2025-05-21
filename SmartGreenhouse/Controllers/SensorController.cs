using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartGreenhouse.Hubs;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Services;

namespace SmartGreenhouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService _sensorService;
        private readonly IHubContext<SensorHub> _hubContext;
        private readonly IGreenhouseService _greenhouseService;

        public SensorController(ISensorService sensorService, IHubContext<SensorHub> hubContext, IGreenhouseService greenhouseService)
        {
            _sensorService = sensorService;
            _hubContext = hubContext;
            _greenhouseService = greenhouseService;
        }

        [HttpPost("readings")]
        public async Task<IActionResult> AddReading([FromBody] SensorReadingCreateDto dto)
        {
            var statusDto = _sensorService.AddSensorReading(dto);  // Ось тут вже оновлений статус

            await _hubContext.Clients
                .Group(dto.GreenhouseId.ToString())
                .SendAsync("GreenhouseStatusUpdated", statusDto);

            return Ok(new { message = "Sensor reading saved and status updated." });
        }



    }
}
