using LibraryManagementSys.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSys.API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateToken(User user)
        {
            try
            {
                // Get JWT settings from appsettings.json
                var secretKey = _configuration["JwtSettings:SecretKey"]!;
                var issuer = _configuration["JwtSettings:Issuer"]!;
                var audience = _configuration["JwtSettings:Audience"]!;
                var expiryInMinutes = int.Parse(
                    _configuration["JwtSettings:ExpiryInMinutes"]!);

                // Create security key
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Create claims — info stored inside the token
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                // Create the token
                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("Token generated for user: {Email}", user.Email);

                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating token for user: {Email}", user.Email);
                throw;
            }
        }
    }
}
