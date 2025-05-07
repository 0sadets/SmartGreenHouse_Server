using Microsoft.AspNetCore.Mvc;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;

namespace SmartGreenhouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorController : Controller
    {
        private readonly ISensorService _sensorService;

        public SensorController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        [HttpPost("readings")]
        public IActionResult AddReading([FromBody] SensorReadingCreateDto dto)
        {
            _sensorService.AddSensorReading(dto);
            return Ok(new { message = "Sensor reading saved successfully." });
        }
    }
}
