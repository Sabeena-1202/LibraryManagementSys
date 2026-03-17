using LibraryManagementSys.API.DTOs;
using LibraryManagementSys.API.Models;
using LibraryManagementSys.API.Repositories;

namespace LibraryManagementSys.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            TokenService tokenService,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Register attempt for email: {Email}",
                    registerDto.Email);

                // Check if user already exists
                var userExists = await _userRepository
                    .UserExistsAsync(registerDto.Email);

                if (userExists)
                {
                    _logger.LogWarning("Registration failed - email already exists: {Email}",
                        registerDto.Email);
                    throw new Exception("User with this email already exists");
                }

                // Create new user
                var user = new User
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                };

                // Save to database
                await _userRepository.CreateUserAsync(user);

                // Generate and return token
                var token = _tokenService.GenerateToken(user);

                _logger.LogInformation("User registered successfully: {Email}",
                    registerDto.Email);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for: {Email}",
                    registerDto.Email);
                throw;
            }
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}",
                    loginDto.Email);

                // Check if user exists
                var user = await _userRepository
                    .GetByEmailAsync(loginDto.Email);

                if (user == null)
                {
                    _logger.LogWarning("Login failed - user not found: {Email}",
                        loginDto.Email);
                    throw new Exception("Invalid email or password");
                }

                // Verify password
                var isPasswordValid = BCrypt.Net.BCrypt
                    .Verify(loginDto.Password, user.Password);

                if (!isPasswordValid)
                {
                    _logger.LogWarning("Login failed - wrong password: {Email}",
                        loginDto.Email);
                    throw new Exception("Invalid email or password");
                }

                // Generate and return token
                var token = _tokenService.GenerateToken(user);

                _logger.LogInformation("User logged in successfully: {Email}",
                    loginDto.Email);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for: {Email}",
                    loginDto.Email);
                throw;
            }
        }
    }
}
