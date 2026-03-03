using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace kursovou_wed.models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Шифровать (используйте BCrypt)
        public string Email { get; set; } // Для уведомлений
        public UserRole Role { get; set; } // Enum: Applicant, Executor, Dispatcher, Admin
        public ICollection<Request> AssignedRequests { get; set; }
    }

    public enum UserRole { Applicant, Executor, Dispatcher, Admin }
}
