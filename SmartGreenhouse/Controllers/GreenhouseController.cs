using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using System.Security.Claims;

namespace SmartGreenhouse.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GreenhouseController : ControllerBase
    {
        private readonly IGreenhouseService _greenhouseService;

        public GreenhouseController(IGreenhouseService greenhouseService)
        {
            _greenhouseService = greenhouseService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGreenhouse([FromBody] GreenhouseCreateDto dto)
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            //if (userIdClaim == null)
            //    return Unauthorized("Користувач не авторизований.");

            int userId = 1; //int.Parse(userIdClaim.Value);

            try
            {
                var greenhouse = await _greenhouseService.CreateWithOptimalSettingsAsync(dto, userId);
                return Ok(greenhouse); // Можеш замінити на DTO, якщо хочеш обмежити поля
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
