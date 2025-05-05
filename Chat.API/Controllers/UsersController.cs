using Chat.Core.Models;
using Chat.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _authService;

        public UsersController(IUsersService authService)
        {
            _authService = authService;
        }

        // POST: api/users/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserCredentialsDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            var result = await _authService.RegisterAsync(registerDto);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = "User registered successfully." });
        }

        // POST: api/users/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            var loginResult = await _authService.LoginAsync(loginDto.Username, loginDto.Password);
            if (string.IsNullOrEmpty(loginResult.Token))
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(loginResult);
        }

        [HttpGet("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var refreshTokenResult = await _authService.RefreshTokenAsync(userName);
            if (refreshTokenResult.Success)
            {
                return Ok(refreshTokenResult);
            }
            return Unauthorized(refreshTokenResult);
        }
       
    }
}

