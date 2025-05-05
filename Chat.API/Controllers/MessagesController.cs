using Chat.Core.Hubs;
using Chat.Core.Models;
using Chat.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUsersService _usersService;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMessageService messageService, IUsersService usersService, ILogger<MessagesController> logger)
        {
            _messageService = messageService;
            _usersService = usersService;
            _logger = logger;
        }

        // GET: api/messages
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll endpoint called to retrieve all messages.");

            try
            {
                var messages = await _messageService.GetAllAsync();
                _logger.LogInformation("Successfully retrieved {Count} messages.", messages.Count());
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving messages.");
                return StatusCode(500, new { message = "An error occurred while retrieving messages." });
            }
        }

        // POST: api/messages
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] MessageCreateDto message)
        {
            _logger.LogInformation("Create endpoint called to add a new message.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid input data for creating a message: {@Message}", message);
                return BadRequest(new { message = "Invalid input data." });
            }

            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                _logger.LogWarning("User not found in claims while creating a message.");
                return Unauthorized(new { message = "User not authenticated." });
            }

            try
            {
                var user = await _usersService.GetByUserNameAsync(userName);
                if (user == null)
                {
                    _logger.LogWarning("User not found in the database: {UserName}", userName);
                    return BadRequest(new { message = "User not found." });
                }

                message.UserId = user.Id;
                var newMessage = await _messageService.CreateAsync(message);

                _logger.LogInformation("Message created successfully with ID: {MessageId} by user: {UserName}", newMessage.Id, userName);
                return CreatedAtAction(nameof(GetById), new { id = newMessage.Id }, newMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a message.");
                return StatusCode(500, new { message = "An error occurred while creating the message." });
            }
        }

        // GET: api/messages/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            _logger.LogInformation("GetById endpoint called for message ID: {MessageId}", id);

            try
            {
                var message = await _messageService.GetByIdAsync(id);
                if (message == null)
                {
                    _logger.LogWarning("Message not found with ID: {MessageId}", id);
                    return NotFound(new { message = "Message not found." });
                }

                _logger.LogInformation("Message retrieved successfully with ID: {MessageId}", id);
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the message with ID: {MessageId}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the message." });
            }
        }

        // DELETE: api/messages/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("Delete endpoint called for message ID: {MessageId}", id);

            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userName))
                {
                    _logger.LogWarning("User not found in claims while deleting a message.");
                    return Unauthorized(new { message = "User not authenticated." });
                }

                await _messageService.DeleteAsync(id);
                _logger.LogInformation("Message deleted successfully with ID: {MessageId} by user: {UserName}", id, userName);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the message with ID: {MessageId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the message." });
            }
        }
    }
}
