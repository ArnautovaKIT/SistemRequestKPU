using kursovou_wed.models;
using System.ComponentModel.DataAnnotations;

namespace SistemRequestKPU.Models
{
    public class UserLoginDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class UserRegisterDTO
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; } // Роль пользователя (как int для enum UserRole)
        public string? AdminApprovalCode { get; set; }
    }
    public class RefreshTokenDTO
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class CreateRequestDto
    {
        [Required] public WorkType WorkType { get; set; }
        [Required] public Priority Priority { get; set; }
        public string ProblemDescription { get; set; } = string.Empty; // Описание проблемы
        public string? AdditionalComments { get; set; } // Комментарии (необязательное)
        [Required][Range(1, int.MaxValue)] public int EquipmentInstanceId { get; set; }
        [Required][Range(1, int.MaxValue)] public int TechnicalObjectId { get; set; }
        [Required][Range(1, int.MaxValue)] public int WorkshopId { get; set; }
        [Range(1, int.MaxValue)] public int? TechnologicalUnitId { get; set; }
        public int? AssigneeId { get; set; }
    }

    public class UpdateRequestDto
    {
        [Required] public WorkType WorkType { get; set; }
        [Required] public Priority Priority { get; set; }
        public string ProblemDescription { get; set; } = string.Empty;
        public string? AdditionalComments { get; set; }
        [Required][Range(1, int.MaxValue)] public int EquipmentInstanceId { get; set; }
        [Required][Range(1, int.MaxValue)] public int TechnicalObjectId { get; set; }
        [Required][Range(1, int.MaxValue)] public int WorkshopId { get; set; }
        [Range(1, int.MaxValue)] public int? TechnologicalUnitId { get; set; }
        public int? AssigneeId { get; set; }
    }

    public class AssignExecutorDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int AssigneeId { get; set; }
    }

}
