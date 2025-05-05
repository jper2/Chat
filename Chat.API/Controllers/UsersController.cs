using Chat.Core.Models;
using Chat.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _authService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUsersService authService, ILogger<UsersController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // POST: api/users/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserCredentialsDto registerDto)
        {
            _logger.LogInformation("Register endpoint called for username: {Username}", registerDto.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid input data for registration: {@RegisterDto}", registerDto);
                return BadRequest(new { message = "Invalid input data." });
            }

            var result = await _authService.RegisterAsync(registerDto);
            if (!result.Success)
            {
                _logger.LogWarning("Registration failed for username: {Username}. Reason: {Message}", registerDto.Username, result.Message);
                return BadRequest(new { message = result.Message });
            }

            _logger.LogInformation("User registered successfully: {Username}", registerDto.Username);
            return Ok(new { message = "User registered successfully." });
        }

        // POST: api/users/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDto loginDto)
        {
            _logger.LogInformation("Login endpoint called for username: {Username}", loginDto.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid input data for login: {@LoginDto}", loginDto);
                return BadRequest(new { message = "Invalid input data." });
            }

            var loginResult = await _authService.LoginAsync(loginDto.Username, loginDto.Password);
            if (string.IsNullOrEmpty(loginResult.Token))
            {
                _logger.LogWarning("Login failed for username: {Username}. Reason: Invalid username or password.", loginDto.Username);
                return Unauthorized(new { message = "Invalid username or password." });
            }

            _logger.LogInformation("User logged in successfully: {Username}", loginDto.Username);
            return Ok(loginResult);
        }

        // GET: api/users/refresh
        [HttpGet("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Refresh token failed. User ID not found in claims.");
                return Unauthorized(new { message = "User ID not found in token." });
            }

            _logger.LogInformation("Refresh token endpoint called for user ID: {UserId}", userId);

            var refreshTokenResult = await _authService.RefreshTokenAsync(userId);
            if (!refreshTokenResult.Success)
            {
                _logger.LogWarning("Refresh token failed for user ID: {UserId}. Reason: {Message}", userId, refreshTokenResult.Message);
                return Unauthorized(refreshTokenResult);
            }

            _logger.LogInformation("Token refreshed successfully for user ID: {UserId}", userId);
            return Ok(refreshTokenResult);
        }
    }
}
