using Chat.Core.Models;
using Chat.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepo;

        public MessagesController(IMessageRepository repository)
        {
            _messageRepo = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _messageRepo.GetAllAsync());

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Message message)
        {
            message.CreatedAt = DateTime.UtcNow;
            await _messageRepo.CreateAsync(message);
            return CreatedAtAction(nameof(GetById), new { id = message.Id }, message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) =>
            Ok(await _messageRepo.GetByIdAsync(id));

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await _messageRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
