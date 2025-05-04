using Chat.Core.Models;
using Chat.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService)
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

            var token = await _authService.LoginAsync(loginDto.Username, loginDto.Password);
            if (string.IsNullOrEmpty(token.Token))
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(new { token });
        }

        //// GET: api/users/me
        //[HttpGet("me")]
        //[Authorize]
        //public async Task<IActionResult> GetCurrentUser()
        //{
        //    var userId = User.Identity?.Name;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized(new { message = "User not authenticated." });
        //    }

        //    var user = await _authService.GetUserByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound(new { message = "User not found." });
        //    }

        //    return Ok(user);
        //}
    }
}

