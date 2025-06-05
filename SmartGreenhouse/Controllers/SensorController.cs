using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartGreenhouse.Hubs;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            var statusDto = _sensorService.AddSensorReading(dto);
            dto.GreenhouseId = (int)_greenhouseService.GetAssignedGreenhouseId("ARDUINO-001");

            await _hubContext.Clients
                .Group(dto.GreenhouseId.ToString())
                .SendAsync("GreenhouseStatusUpdated", new
                {
                    //status = statusDto.Status,
                    status = new
                    {
                        statusDto.Status,
                        statusDto.Alerts,
                    },
                    data = new
                    {
                        dto.GreenhouseId,
                        dto.AirTemp,
                        dto.AirHum,
                        dto.SoilHum,
                        dto.SoilTemp,
                        dto.LightLevel
                    }
                });


            Console.WriteLine($"Надсилаємо статус до групи {dto.GreenhouseId}: {statusDto.Status}, {statusDto.Alerts}");
            Console.WriteLine($"Надсилаємо дані  до групи {dto.GreenhouseId}: {dto.AirHum}, {dto.LightLevel}, {dto.AirTemp}, {dto.SoilHum}, {dto.SoilTemp}");


            return Ok(new { message = "Sensor reading saved and status updated." });
        }
        [HttpGet("latest/{greenhouseId}")]
        public ActionResult<SensorReadDto> GetLatestSensorData(int greenhouseId)
        {
            try
            {
                var data = _sensorService.ReadSensorDataById(greenhouseId);
                return Ok(data);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Внутрішня помилка сервера", details = ex.Message });
            }
        }




    }
}
