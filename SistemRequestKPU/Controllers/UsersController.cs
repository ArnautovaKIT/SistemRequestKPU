using kursovou_wed.models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MimeKit;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SistemRequestKPU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Dispatcher,Admin")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _memoryCache;

        public UsersController(ApplicationDbContext context, IConfiguration config, IMemoryCache memoryCache)
        {
            _context = context;
            _config = config;
            _memoryCache = memoryCache;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    Role = u.Role.ToString(),
                    u.Email
                })
                .ToListAsync();
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    Role = u.Role.ToString(),
                    u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("admin-registration-code")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SendAdminRegistrationCode([FromBody] AdminRegistrationCodeRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Для отправки кода укажите логин и email");

            var approverEmail = _config["AdminRegistrationApproval:ApproverEmail"];
            if (string.IsNullOrWhiteSpace(approverEmail))
                return BadRequest("В конфигурации не задан AdminRegistrationApproval:ApproverEmail");

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            var cacheKey = GetAdminApprovalCacheKey(dto.Username, dto.Email);
            var ttlMinutes = int.TryParse(_config["AdminRegistrationApproval:CodeLifetimeMinutes"], out var parsedMinutes) ? parsedMinutes : 10;

            _memoryCache.Set(cacheKey, code, TimeSpan.FromMinutes(ttlMinutes));

            var message = $"Код подтверждения для регистрации администратора\n" +
                          $"Логин: {dto.Username}\n" +
                          $"Email: {dto.Email}\n" +
                          $"Код: {code}\n" +
                          $"Срок действия: {ttlMinutes} минут.";

            await SendNotification(approverEmail, "Подтверждение регистрации администратора", message);

            return Ok(new { message = "Код подтверждения отправлен на email согласования" });
        }

        // POST: api/users
        [HttpPost]
        [Authorize(Roles = "Admin,Dispatcher")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            // Проверка на существование пользователя с таким логином или email
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("Пользователь с таким логином уже существует");

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Пользователь с таким email уже существует");

            // Проверка прав: Диспетчер может создавать только Исполнителей
            var currentRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (currentRole == "Dispatcher" && dto.Role != UserRole.Executor)
                return Forbid("Диспетчер может создавать только исполнителей");

            if (dto.Role == UserRole.Admin)
            {
                if (currentRole != "Admin")
                    return Forbid("Создание администратора доступно только администратору");

                if (string.IsNullOrWhiteSpace(dto.AdminApprovalCode))
                    return BadRequest("Для регистрации администратора требуется код подтверждения");

                var cacheKey = GetAdminApprovalCacheKey(dto.Username, dto.Email);
                if (!_memoryCache.TryGetValue<string>(cacheKey, out var expectedCode))
                    return BadRequest("Код подтверждения не найден или истек. Запросите новый код.");

                if (!string.Equals(expectedCode, dto.AdminApprovalCode.Trim(), StringComparison.Ordinal))
                    return BadRequest("Неверный код подтверждения администратора");

                _memoryCache.Remove(cacheKey);
            }

            // Создание нового пользователя
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Email = dto.Email,
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Возвращаем упрощенный объект без циклических ссылок
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new
            {
                user.Id,
                user.Username,
                Role = user.Role.ToString(),
                user.Email
            });
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            // Проверка прав: Диспетчер может редактировать только Исполнителей
            var currentRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (currentRole == "Dispatcher" && dto.Role != UserRole.Executor)
                return Forbid("Диспетчер может редактировать только исполнителей");

            // Проверка уникальности логина и email (кроме текущего пользователя)
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username && u.Id != id))
                return BadRequest("Пользователь с таким логином уже существует");

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                return BadRequest("Пользователь с таким email уже существует");

            // Обновление данных
            user.Username = dto.Username;
            user.Email = dto.Email;
            user.Role = dto.Role;

            // Обновляем пароль только если он указан
            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            // Нельзя удалить самого себя
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            if (id == currentUserId)
                return BadRequest("Нельзя удалить свою собственную учетную запись");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private string GetAdminApprovalCacheKey(string username, string email)
            => $"admin-registration:{username.Trim().ToLowerInvariant()}:{email.Trim().ToLowerInvariant()}";

        private async Task SendNotification(string toEmail, string subject, string message)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(emailSettings["SenderEmail"]));
            mimeMessage.To.Add(MailboxAddress.Parse(toEmail));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("plain") { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync(emailSettings["SmtpServer"], 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailSettings["Username"], emailSettings["Password"]);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }

    // DTO для создания пользователя
    public class CreateUserDto
    {
        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required, EmailAddress, StringLength(256)]
        public string Email { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public string? AdminApprovalCode { get; set; }
    }

    public class AdminRegistrationCodeRequestDto
    {
        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, EmailAddress, StringLength(256)]
        public string Email { get; set; }
    }

    // DTO для обновления пользователя
    public class UpdateUserDto
    {
        [Required, StringLength(100)]
        public string Username { get; set; }

        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required, EmailAddress, StringLength(256)]
        public string Email { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
