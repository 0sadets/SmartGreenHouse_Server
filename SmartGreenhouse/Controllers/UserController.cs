using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartGreenhouse.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(new { message = "Користувач не авторизований" });

            int userId = int.Parse(userIdClaim.Value);
            var userDto = await _userService.GetUserInfoAsync(userId);
            return Ok(userDto);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(new { message = "Користувач не авторизований" });

            int userId = int.Parse(userIdClaim.Value);

            try
            {
                await _userService.UpdateUserAsync(userId, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(new { message = "Користувач не авторизований" });

            int userId = int.Parse(userIdClaim.Value);

            try
            {
                await _userService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }

}
