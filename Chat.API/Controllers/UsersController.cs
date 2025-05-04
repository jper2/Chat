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

            var loginResult = await _authService.LoginAsync(loginDto.Username, loginDto.Password);
            if (string.IsNullOrEmpty(loginResult.Token))
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(loginResult);
        }

        [HttpGet("refresh")]
        [Authorize]
        public IActionResult RefreshToken()
        {
            // Get the token from the Authorization header
            var authHeader = Request.Headers["Authorization"].ToString();
            
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Authorization header is missing or invalid." });
            }

            // Extract the token (remove "Bearer " prefix)
            var token = authHeader.Substring("Bearer ".Length).Trim();

            //var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            //var userName = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;






            var newToken = _authService.RefreshTokenAsync(userName);
            if (newToken == null)
            {
                return Unauthorized(new { message = "Invalid or expired token." });
            }

            return Ok(new { token = newToken });










            //var newToken = _authService.RefreshToken(token);
            //if (newToken == null)
            //{
            //    return Unauthorized(new { message = "Invalid or expired token." });
            //}

            //return Ok(new { token = newToken });
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

