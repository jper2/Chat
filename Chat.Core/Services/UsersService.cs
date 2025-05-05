using Chat.Core.Models;
using Chat.Core.Repositories;
using Chat.Core.Utilities;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Chat.Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly JwtSettings _jwtSettings;

        public UsersService(IUsersRepository authRepository, IOptions<JwtSettings> jwtSettings)
        {
            _usersRepository = authRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResult> RegisterAsync(UserCredentialsDto userRegisterDto)
        {
            var existingUser = await _usersRepository.GetByUsernameAsync(userRegisterDto.Username);
            if (existingUser != null)
            {
                return new AuthResult { Success = false, Message = "User already exists." };
            }

            var (hash, salt) = PasswordHasher.CreateHash(userRegisterDto.Password);
            var newUser = new User
            {
                Username = userRegisterDto.Username,
                PasswordHash = hash,
                Salt = salt,
                CreatedAt = DateTime.UtcNow
            };

            await _usersRepository.CreateUserAsync(newUser);
            return new AuthResult { Success = true, Message = "User registered successfully." };
        }

        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password must not be empty.");
            }
            var user = await _usersRepository.GetByUsernameAsync(username);
            if (user == null || !PasswordHasher.VerifyHash(password, user.PasswordHash, user.Salt))
            {
                return new AuthResult { Success = false, Message = "Invalid username or password." };
            }

            var token = JwtHelper.GenerateJwtToken(user, _jwtSettings);
            return new AuthResult { Success = true, Token = token };
        }

        public async Task<AuthResult> RefreshTokenAsync(string userName)
        {
            var result = new AuthResult { Success = false, Message = "Invalid token." };
            try
            {
                var user = await _usersRepository.GetByUsernameAsync(userName);
                if (user != null)
                {
                    var newToken = JwtHelper.GenerateJwtToken(user, _jwtSettings);
                    return new AuthResult { Success = true, Token = newToken };
                }
            }
            catch (Exception ex)
            {
                //Log the exception
            }
            return new AuthResult { Success = false, Message = "Invalid token." };
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("User ID must not be empty.");
            }
            return await _usersRepository.GetByUsernameAsync(userName.ToLower());
        }
    }
}
