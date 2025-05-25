using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;
using SmartGreenhouse.Validation;
using System.Security.Claims;

namespace SmartGreenhouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authService,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _userRepository = userRepository;
        }
        //[Authorize]
        //[HttpGet("whoami")]
        //public IActionResult WhoAmI()
        //{
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized("User ID claim is missing");
        //    }
        //    return Ok($"Ваш ID: {userIdClaim.Value}");

        //}
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var validator = new RegisterDtoValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = new AppUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var accessToken = await _authService.GenerateAccessToken(user);
            var refreshToken = await _authService.GenerateAndStoreRefreshToken(user);

            return Ok(new { accessToken, refreshToken });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid credentials");

            var accessToken = await _authService.GenerateAccessToken(user);
            var refreshToken = await _authService.GenerateAndStoreRefreshToken(user);

            return Ok(new { accessToken, refreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            try
            {
                var (accessToken, refreshToken) = await _authService.RefreshTokens(dto.RefreshToken);
                return Ok(new { accessToken, refreshToken });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto)
        {
            var token = await _userRepository.GetRefreshTokenAsync(dto.RefreshToken);
            if (token == null)
                return NotFound("Refresh token not found");

            await _userRepository.RevokeRefreshTokenAsync(token);
            await _userRepository.SaveAsync();

            return Ok("Logged out successfully");
        }
        [Authorize]
        [HttpGet("validate-token")]
        public IActionResult ValidateToken()
        {
            return Ok(new { valid = true });
        }

    }

}
