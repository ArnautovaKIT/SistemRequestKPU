using System.ComponentModel.DataAnnotations;

namespace kursovou_wed.models
{
    public class Request
    {
        public int Id { get; set; }
        public string UniqueNumber { get; set; } = Guid.NewGuid().ToString(); // Авто-генерация
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public WorkType WorkType { get; set; } // Enum: Установка, Наладка, Ремонт
        public Priority Priority { get; set; } // Enum: Низкий, Средний, Высокий, Критический
        public RequestStatus Status { get; set; } = RequestStatus.New; // Enum: New, InProgress, Completed, Rejected
        public string TechnicalSpecs { get; set; } // Технические характеристики
        public string? Requirements { get; set; } // Требования

        public int EquipmentInstanceId { get; set; } // FK на EquipmentInstance (замена EquipmentId)
        public EquipmentInstance EquipmentInstance { get; set; } = null!;

        public int TechnicalObjectId { get; set; } // FK на TechnicalObject (замена ObjectId)
        public TechnicalObject TechnicalObject { get; set; } = null!;

        public int? AssigneeId { get; set; } // Назначенный исполнитель
        public User? Assignee { get; set; }
        public int CreatorId { get; set; } // Заявитель
        public User Creator { get; set; }

        public int WorkshopId { get; set; }
        public Workshop Workshop { get; set; } = null!;
        public int? TechnologicalUnitId { get; set; }
        public TechnologicalUnit? TechnologicalUnit { get; set; }
    }

    public enum WorkType { Installation, Setup, Repair }
    public enum Priority { Low, Medium, High, Critical }
    public enum RequestStatus { New, InProgress, Completed, Rejected }

   

    
}
