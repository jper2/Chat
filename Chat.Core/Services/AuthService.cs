using Chat.Core.Models;
using Chat.Core.Repositories;
using Chat.Core.Utilities;

namespace Chat.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IAuthRepository authRepository, JwtSettings jwtSettings)
        {
            _authRepository = authRepository;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthResult> Register(UserCredentialsDto userRegisterDto)
        {
            var existingUser = await _authRepository.GetByUsernameAsync(userRegisterDto.Username);
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
                Roles = new List<string> { "User" },
                CreatedAt = DateTime.UtcNow
            };

            await _authRepository.CreateUserAsync(newUser);
            return new AuthResult { Success = true, Message = "User registered successfully." };
        }

        public async Task<AuthResult> Login(string username, string password)
        {
            var user = await _authRepository.GetByUsernameAsync(username);
            if (user == null || !PasswordHasher.VerifyHash(password, user.PasswordHash, user.Salt))
            {
                return new AuthResult { Success = false, Message = "Invalid username or password." };
            }

            var token = JwtHelper.GenerateJwtToken(user, _jwtSettings);
            return new AuthResult { Success = true, Token = token };
        }

    }

}
