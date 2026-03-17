using LibraryManagementSys.API.DTOs;

namespace LibraryManagementSys.API.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}