using kursovou_wed.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SistemRequestKPU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Dispatcher,Admin")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public UsersController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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
