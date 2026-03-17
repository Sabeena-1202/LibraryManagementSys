using LibraryManagementSys.API.Models;

namespace LibraryManagementSys.API.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<bool> UserExistsAsync(string email);
    }
}