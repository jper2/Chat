using Chat.Core.Models;
using Chat.Core.Repositories;
using Chat.Core.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chat.Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger _logger;

        public UsersService(IUsersRepository usersRepository, IOptions<JwtSettings> jwtSettings, ILogger<UsersService> logger)
        {
            _usersRepository = usersRepository;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<AuthResult> RegisterAsync(UserCredentialsDto userRegisterDto)
        {
            _logger.LogInformation("RegisterAsync called for username: {Username}", userRegisterDto.Username);

            if (string.IsNullOrWhiteSpace(userRegisterDto.Username) || string.IsNullOrWhiteSpace(userRegisterDto.Password))
            {
                _logger.LogWarning("Invalid input data for registration: {@UserRegisterDto}", userRegisterDto);
                return new AuthResult { Success = false, Message = "Username and password must not be empty." };
            }

            var existingUser = await _usersRepository.GetByUsernameAsync(userRegisterDto.Username.ToLowerInvariant());
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed. User already exists: {Username}", userRegisterDto.Username);
                return new AuthResult { Success = false, Message = "User already exists." };
            }

            var (hash, salt) = PasswordHasher.CreateHash(userRegisterDto.Password);
            var newUser = new User
            {
                Username = userRegisterDto.Username.ToLowerInvariant(),
                PasswordHash = hash,
                Salt = salt,
                CreatedAt = DateTime.UtcNow
            };

            await _usersRepository.CreateUserAsync(newUser);
            _logger.LogInformation("User registered successfully: {Username}", userRegisterDto.Username);
            return new AuthResult { Success = true, Message = "User registered successfully." };
        }

        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            _logger.LogInformation("LoginAsync called for username: {Username}", username);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Invalid input data for login. Username or password is empty.");
                return new AuthResult { Success = false, Message = "Username and password must not be empty." };
            }

            var user = await _usersRepository.GetByUsernameAsync(username.ToLowerInvariant());
            if (user == null || !PasswordHasher.VerifyHash(password, user.PasswordHash, user.Salt))
            {
                _logger.LogWarning("Login failed for username: {Username}. Invalid username or password.", username);
                return new AuthResult { Success = false, Message = "Invalid username or password." };
            }

            var token = JwtHelper.GenerateJwtToken(user, _jwtSettings);
            _logger.LogInformation("User logged in successfully: {Username}", username);
            return new AuthResult { Success = true, Token = token };
        }

        public async Task<AuthResult> RefreshTokenAsync(string userName)
        {
            _logger.LogInformation("RefreshTokenAsync called for username: {UserName}", userName);

            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.LogWarning("Invalid input data for token refresh. Username is empty.");
                return new AuthResult { Success = false, Message = "Invalid token." };
            }

            try
            {
                var user = await _usersRepository.GetByUsernameAsync(userName.ToLowerInvariant());
                if (user != null)
                {
                    var newToken = JwtHelper.GenerateJwtToken(user, _jwtSettings);
                    _logger.LogInformation("Token refreshed successfully for username: {UserName}", userName);
                    return new AuthResult { Success = true, Token = newToken };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing the token for username: {UserName}", userName);
            }

            _logger.LogWarning("Token refresh failed for username: {UserName}", userName);
            return new AuthResult { Success = false, Message = "Invalid token." };
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            _logger.LogInformation("GetByUserNameAsync called for username: {UserName}", userName);

            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.LogWarning("Invalid input data for GetByUserNameAsync. Username is empty.");
                throw new ArgumentException("Username must not be empty.");
            }

            return await _usersRepository.GetByUsernameAsync(userName.ToLowerInvariant());
        }
    }
}

