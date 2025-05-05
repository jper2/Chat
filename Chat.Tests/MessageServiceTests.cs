using Chat.Core.Hubs;
using Chat.Core.Models;
using Chat.Core.Repositories;
using Chat.Core.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Chat.Tests
{
    public class MessageServiceTests
    {
        private readonly Mock<IMessageRepository> _messageRepositoryMock;
        private readonly Mock<IHubContext<ChatHub>> _hubContextMock;
        private readonly Mock<ILogger<MessageService>> _loggerMock;
        private readonly MessageService _messageService;

        public MessageServiceTests()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _hubContextMock = new Mock<IHubContext<ChatHub>>();
            _loggerMock = new Mock<ILogger<MessageService>>();

            _messageService = new MessageService(_messageRepositoryMock.Object, _hubContextMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMessage_WhenValidInput()
        {
            // Arrange
            var messageDto = new MessageCreateDto
            {
                Content = "Hello, world!",
                Type = "Text",
                UserId = "12345"
            };

            var createdMessage = new Message
            {
                Id = "1",
                Content = "Hello, world!",
                Type = "Text",
                UserId = "12345",
                CreatedAt = DateTime.UtcNow
            };

            _messageRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Message>())).Callback<Message>(m => m.Id = "1").Returns(Task.CompletedTask);

            // Act
            var result = await _messageService.CreateAsync(messageDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("Hello, world!", result.Content);
            _messageRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Message>()), Times.Once);
        }
    }
}