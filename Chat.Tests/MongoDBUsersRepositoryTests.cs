using Chat.Core.Models;
using Chat.Core.Repositories;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Chat.Tests
{
    public class MongoDBUsersRepositoryTests
    {
        private readonly Mock<IMongoCollection<User>> _usersCollectionMock;
        private readonly Mock<IMongoClient> _mongoClientMock;
        private readonly Mock<ILogger<MongoDBUsersRepository>> _loggerMock;
        private readonly MongoDBUsersRepository _usersRepository;

        public MongoDBUsersRepositoryTests()
        {
            _usersCollectionMock = new Mock<IMongoCollection<User>>();
            _mongoClientMock = new Mock<IMongoClient>();
            _loggerMock = new Mock<ILogger<MongoDBUsersRepository>>();

            var databaseMock = new Mock<IMongoDatabase>();
            databaseMock.Setup(d => d.GetCollection<User>("Users", null)).Returns(_usersCollectionMock.Object);

            _mongoClientMock.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(databaseMock.Object);

            _usersRepository = new MongoDBUsersRepository(_mongoClientMock.Object, new Mock<IMongoDbSettings>().Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var username = "johndoe";
            var user = new User { Username = username };

            // Mock the IFindFluent<TDocument, TProjection> returned by Find
            var findFluentMock = new Mock<IFindFluent<User, User>>();
            findFluentMock.Setup(f => f.FirstOrDefaultAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(user);

            // Mock the Find method to return the mocked IFindFluent
            _usersCollectionMock.Setup(c => c.Find(It.IsAny<FilterDefinition<User>>(), null))
                                .Returns(findFluentMock.Object);

            // Act
            var result = await _usersRepository.GetByUsernameAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }
    }
}

