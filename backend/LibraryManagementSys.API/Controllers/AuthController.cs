using LibraryManagementSys.API.DTOs;
using LibraryManagementSys.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSys.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid registration model state");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Register request for: {Email}",
                    registerDto.Email);

                var token = await _authService.RegisterAsync(registerDto);

                return Ok(new
                {
                    Message = "Registration successful",
                    Token = token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for: {Email}",
                    registerDto.Email);
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid login model state");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Login request for: {Email}",
                    loginDto.Email);

                var token = await _authService.LoginAsync(loginDto);

                return Ok(new
                {
                    Message = "Login successful",
                    Token = token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for: {Email}",
                    loginDto.Email);
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}