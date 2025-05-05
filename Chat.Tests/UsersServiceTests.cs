using Chat.Core.Models;
using Chat.Core.Repositories;
using Chat.Core.Services;
using Chat.Core.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Chat.Tests
{
    public class UsersServiceTests
    {
        private readonly Mock<IUsersRepository> _usersRepositoryMock;
        private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock;
        private readonly Mock<ILogger<UsersService>> _loggerMock;
        private readonly UsersService _usersService;

        public UsersServiceTests()
        {
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
            _loggerMock = new Mock<ILogger<UsersService>>();

            _jwtSettingsMock.Setup(j => j.Value).Returns(new JwtSettings
            {
                Key = "test-key",
                Issuer = "test-issuer",
                Audience = "test-audience",
                ExpiryMinutes = 60
            });

            _usersService = new UsersService(_usersRepositoryMock.Object, _jwtSettingsMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccess_WhenUserIsNew()
        {
            // Arrange
            var userDto = new UserCredentialsDto { Username = "newuser", Password = "password123" };
            _usersRepositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act
            var result = await _usersService.RegisterAsync(userDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("User registered successfully.", result.Message);
            _usersRepositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFailure_WhenUserAlreadyExists()
        {
            // Arrange
            var userDto = new UserCredentialsDto { Username = "existinguser", Password = "password123" };
            _usersRepositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(new User());

            // Act
            var result = await _usersService.RegisterAsync(userDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User already exists.", result.Message);
            _usersRepositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }
    }
}