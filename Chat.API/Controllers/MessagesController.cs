using Chat.Core.Hubs;
using Chat.Core.Models;
using Chat.Core.Repositories;
using Chat.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUsersService _usersService;

        public MessagesController(IMessageService messageservice, IUsersService authService)
        {
            _messageService = messageservice;
            _usersService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _messageService.GetAllAsync());

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] MessageCreateDto message)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            
            var user = await _usersService.GetByUserNameAsync(userName);
            if (user != null)
            {
                message.UserId = user.Id;
                var newMessage = await _messageService.CreateAsync(message);
                return CreatedAtAction(nameof(GetById), new { id = newMessage.Id }, newMessage);
            }
            else
            {
                return BadRequest(new { message = "User not found." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) =>
            Ok(await _messageService.GetByIdAsync(id));

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await _messageService.DeleteAsync(id);
            return NoContent();
        }
    }
}
