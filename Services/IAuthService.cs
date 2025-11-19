using EduMaster.Models;

namespace EduMaster.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(string username, string email, string password, string? fullName = null);
        Task<User?> LoginAsync(string emailOrUsername, string password);
        Task<bool> UserExistsAsync(string username, string email);
        Task UpdateLastLoginAsync(int userId);
    }
}