using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using System.Security.Claims;

namespace SmartGreenhouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreenhouseController : ControllerBase
    {
        private readonly IGreenhouseService _greenhouseService;

        public GreenhouseController(IGreenhouseService greenhouseService)
        {
            _greenhouseService = greenhouseService;
        }
        [Authorize]
        [HttpGet("test")]
        public IActionResult Test() => Ok("Працює");



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
    }
}
