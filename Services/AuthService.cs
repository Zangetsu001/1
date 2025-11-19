using EduMaster.Data;
using EduMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace EduMaster.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(ApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> RegisterAsync(string username, string email, string password, string? fullName = null)
        {
            // Проверяем, существует ли пользователь
            if (await UserExistsAsync(username, email))
            {
                return null;
            }

            // Создаем нового пользователя
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = _passwordHasher.HashPassword(password),
                FullName = fullName,
                Role = "Student",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Создаем профиль пользователя
            var profile = new UserProfile
            {
                UserId = user.Id,
                UpdatedAt = DateTime.UtcNow
            };

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> LoginAsync(string emailOrUsername, string password)
        {
            // Ищем пользователя по email или username
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    (u.Email == emailOrUsername || u.Username == emailOrUsername)
                    && u.IsActive);

            if (user == null)
            {
                return null;
            }

            // Проверяем пароль
            if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            // Обновляем время последнего входа
            await UpdateLastLoginAsync(user.Id);

            return user;
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username || u.Email == email);
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}