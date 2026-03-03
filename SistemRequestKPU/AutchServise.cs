using kursovou_wed.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemRequestKPU.Models;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemRequestKPU
{
    public interface IAuthService
    {
        Task<(string? AccessToken, string? RefreshToken)> Authenticate(UserLoginDTO loginDto);
        Task<(bool Success, string? ErrorMessage)> RegisterUser(UserRegisterDTO registerDto);
    }
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        public AuthService(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<(string? AccessToken, string? RefreshToken)> Authenticate(UserLoginDTO loginDto)
        {
            try
            {
                var user = await _context.Users
                .Where(u => u.Username == loginDto.Username)
                .Select(u => new { u.Id, u.Username, u.PasswordHash, RoleName = u.Role.ToString() })
                .FirstOrDefaultAsync();
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Неудачная попытка входа для пользователя: {Username}", loginDto.Username);
                    return (null, null);
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
                var tokenLifetimeHours = double.Parse(_configuration["Jwt:LifetimeHours"] ?? "2", CultureInfo.InvariantCulture);
                // Устанавливаем текущее время для iat и exp
                var now = DateTimeOffset.UtcNow;
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.RoleName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("iat", now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                    }),
                    NotBefore = now.UtcDateTime, // nbf
                    Expires = now.AddHours(tokenLifetimeHours).UtcDateTime, // exp
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"]
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = tokenHandler.WriteToken(token);
                var refreshToken = Guid.NewGuid().ToString();
                _context.RefreshTokens.Add(new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    Expires = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                });
                await _context.SaveChangesAsync();
                _logger.LogInformation("Токен выдан для пользователя: {Username}. AccessToken: {AccessToken}, RefreshToken: {RefreshToken}",
                user.Username, accessToken, refreshToken);
                return (accessToken, refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при аутентификации пользователя: {Username}", loginDto.Username);
                return (null, null);
            }
        }
        public async Task<(bool Success, string? ErrorMessage)> RegisterUser(UserRegisterDTO registerDto)
        {
            try
            {
                // Валидация имени
                if (string.IsNullOrWhiteSpace(registerDto.Username) || registerDto.Username.Length < 3)
                    return (false, "Имя пользователя должно содержать минимум 3 символа.");

                // Валидация пароля
                if (string.IsNullOrWhiteSpace(registerDto.Password) || registerDto.Password.Length < 8)
                    return (false, "Пароль должен содержать минимум 8 символов.");

                // Проверка: пользователь уже существует?
                if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                    return (false, "Пользователь с таким именем уже существует.");

                // Проверка: корректная роль? (0..3)
                if (registerDto.RoleId < 0 || registerDto.RoleId > 3)
                    return (false, "Роль должна быть от 0 до 3 (0=Applicant, 1=Executor, 2=Dispatcher, 3=Admin)");

                // Всё ок — создаём пользователя
                var newUser = new User
                {
                    Username = registerDto.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    Email = $"{registerDto.Username}@example.com", // или оставьте пустым, если не нужно
                    Role = (UserRole)registerDto.RoleId
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Зарегистрирован пользователь: {Username} с ролью {Role}",
                    registerDto.Username, newUser.Role);

                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя {Username}", registerDto.Username);
                return (false, $"Внутренняя ошибка: {ex.Message}");
            }
        }
    }

}
