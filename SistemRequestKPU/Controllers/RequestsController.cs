using kursovou_wed.models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SistemRequestKPU.Models;

using System.Security.Claims;

namespace SistemRequestKPU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Требует JWT
    public class RequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public RequestsController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        [HttpPost]
        [Authorize(Roles = "Applicant,Dispatcher,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var creatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            // Загружаем связанные данные с Include
            var equipmentInstance = await _context.EquipmentInstances
                .Include(ei => ei.EquipmentType)
                .FirstOrDefaultAsync(ei => ei.Id == dto.EquipmentInstanceId);

            if (equipmentInstance == null) return BadRequest("Invalid EquipmentInstanceId");

            // Проверяем, что EquipmentType загружен
            if (equipmentInstance.EquipmentType == null)
                return BadRequest("Invalid EquipmentType for selected EquipmentInstance");

            var technicalObject = await _context.TechnicalObjects
                .Include(to => to.Complex)
                .FirstOrDefaultAsync(to => to.Id == dto.TechnicalObjectId);

            if (technicalObject == null) return BadRequest("Invalid TechnicalObjectId");

            var workshop = await _context.Workshops.FindAsync(dto.WorkshopId);
            if (workshop == null) return BadRequest("Invalid WorkshopId");

            TechnologicalUnit technologicalUnit = null;
            if (dto.TechnologicalUnitId.HasValue)
            {
                technologicalUnit = await _context.TechnologicalUnits.FindAsync(dto.TechnologicalUnitId.Value);
                if (technologicalUnit == null)
                    return BadRequest("Invalid TechnologicalUnitId");
            }

            User assignee = null;
            if (dto.AssigneeId.HasValue)
            {
                assignee = await _context.Users.FindAsync(dto.AssigneeId.Value);
                if (assignee == null || assignee.Role != UserRole.Executor)
                    return BadRequest("Invalid AssigneeId or user is not an Executor");
            }

            var creator = await _context.Users.FindAsync(creatorId);
            if (creator == null)
                return BadRequest("Creator not found");

            var request = new Request
            {
                UniqueNumber = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                WorkType = dto.WorkType,
                Priority = dto.Priority,
                Status = RequestStatus.New,
                TechnicalSpecs = dto.ProblemDescription,
                Requirements = dto.AdditionalComments,
                EquipmentInstanceId = dto.EquipmentInstanceId,
                EquipmentInstance = equipmentInstance,
                TechnicalObjectId = dto.TechnicalObjectId,
                TechnicalObject = technicalObject,
                WorkshopId = dto.WorkshopId,
                Workshop = workshop,
                TechnologicalUnitId = dto.TechnologicalUnitId,
                TechnologicalUnit = technologicalUnit,
                AssigneeId = dto.AssigneeId,
                Assignee = assignee,
                CreatorId = creatorId,
                Creator = creator
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            // Возвращаем упрощенный объект с проверками на null
            return Ok(new
            {
                request.Id,
                request.UniqueNumber,
                CreatedAt = request.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                WorkType = request.WorkType.ToString(),
                Priority = request.Priority.ToString(),
                Status = request.Status.ToString(),
                request.TechnicalSpecs,
                request.Requirements,
                EquipmentInstanceId = request.EquipmentInstanceId,
                TechnicalObjectId = request.TechnicalObjectId,
                WorkshopId = request.WorkshopId,
                TechnologicalUnitId = request.TechnologicalUnitId,
                AssigneeId = request.AssigneeId,
                CreatorId = request.CreatorId,
                // Данные для отображения с проверками на null
                EquipmentInstance = new
                {
                    equipmentInstance.Id,
                    Name = equipmentInstance.EquipmentType != null ?
                           $"{equipmentInstance.EquipmentType.Name} ({(string.IsNullOrEmpty(equipmentInstance.FactoryNumber) ? equipmentInstance.InventoryNumber : equipmentInstance.FactoryNumber)})" :
                           "Неизвестное оборудование",
                    EquipmentType = equipmentInstance.EquipmentType?.Name
                },
                TechnicalObject = new
                {
                    technicalObject.Id,
                    technicalObject.Name,
                    ComplexName = technicalObject.Complex?.Name
                },
                Assignee = assignee != null ? new
                {
                    assignee.Id,
                    assignee.Username
                } : null,
                Creator = new
                {
                    creator.Id,
                    creator.Username
                },
                Workshop = new
                {
                    workshop.Id,
                    Name = $"{workshop.Name} ({workshop.Code})"
                },
                TechnologicalUnit = technologicalUnit != null ? new
                {
                    technologicalUnit.Id,
                    technologicalUnit.Name
                } : null
            });
        }
        // Получение всех с поиском/фильтрацией/сортировкой
        [HttpGet]
        public async Task<IActionResult> GetAll(string? search, WorkType? workType, RequestStatus? status, DateTime? fromDate, string sortBy = "CreatedAt", bool ascending = true)
        {
            var query = _context.Requests
        .Include(r => r.EquipmentInstance)
            .ThenInclude(ei => ei.EquipmentType)
        .Include(r => r.TechnicalObject)
            .ThenInclude(to => to.Complex)
        .Include(r => r.Assignee)
        .Include(r => r.Creator)
        .Include(r => r.Workshop)
        .Include(r => r.TechnologicalUnit)
        .AsQueryable();

            // Применяем фильтры
            if (!string.IsNullOrEmpty(search))
                query = query.Where(r => r.TechnicalSpecs.Contains(search) ||
                                        (r.Requirements != null && r.Requirements.Contains(search)));
            if (workType.HasValue)
                query = query.Where(r => r.WorkType == workType.Value);
            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);
            if (fromDate.HasValue)
                query = query.Where(r => r.CreatedAt >= fromDate.Value);

            // Сортируем
            query = sortBy switch
            {
                "WorkType" => ascending ? query.OrderBy(r => r.WorkType) : query.OrderByDescending(r => r.WorkType),
                "Priority" => ascending ? query.OrderBy(r => r.Priority) : query.OrderByDescending(r => r.Priority),
                "Status" => ascending ? query.OrderBy(r => r.Status) : query.OrderByDescending(r => r.Status),
                "CreatedAt" => ascending ? query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt),
                _ => ascending ? query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt)
            };

            // Возвращаем проекцию без циклических ссылок
            var result = await query.Select(r => new
            {
                r.Id,
                r.UniqueNumber,
                CreatedAt = r.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                r.WorkType,
                r.Priority,
                r.Status,
                TechnicalSpecs = r.TechnicalSpecs,
                Requirements = r.Requirements, // Теперь может быть null
                EquipmentInstance = r.EquipmentInstance != null && r.EquipmentInstance.EquipmentType != null ? new
                {
                    r.EquipmentInstance.Id,
                    Name = $"{r.EquipmentInstance.EquipmentType.Name} ({(string.IsNullOrEmpty(r.EquipmentInstance.FactoryNumber) ? r.EquipmentInstance.InventoryNumber : r.EquipmentInstance.FactoryNumber)})",
                    EquipmentType = r.EquipmentInstance.EquipmentType.Name
                } : null,
                TechnicalObject = new
                {
                    r.TechnicalObject.Id,
                    Name = r.TechnicalObject.Name,
                    ComplexName = r.TechnicalObject.Complex.Name
                },
                Assignee = r.Assignee != null ? new
                {
                    r.Assignee.Id,
                    r.Assignee.Username
                } : null,
                Creator = new
                {
                    r.Creator.Id,
                    r.Creator.Username
                },
                Workshop = new
                {
                    r.Workshop.Id,
                    Name = r.Workshop.Name + " (" + r.Workshop.Code + ")"
                },
                TechnologicalUnit = r.TechnologicalUnit != null ? new
                {
                    r.TechnologicalUnit.Id,
                    r.TechnologicalUnit.Name
                } : null
            }).ToListAsync();

            return Ok(result);
        }
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Dispatcher,Executor,Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] RequestStatus newStatus)
        {
            var request = await _context.Requests
                .Include(r => r.Creator)
                .Include(r => r.EquipmentInstance).ThenInclude(ei => ei.EquipmentType)
                .Include(r => r.TechnicalObject).ThenInclude(to => to.Complex)
                .Include(r => r.Workshop)
                .Include(r => r.TechnologicalUnit)
                .Include(r => r.Assignee)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();

            var oldStatus = request.Status;
            request.Status = newStatus;
            await _context.SaveChangesAsync();

            // Отправка уведомления
            var message = $@"Уважаемый {request.Creator.Username}!
        Статус вашей заявки изменён:
        Номер заявки: {request.UniqueNumber}
        Оборудование: {request.EquipmentInstance?.EquipmentType.Name ?? "не указано"}
        Объект: {request.TechnicalObject?.Name ?? "не указано"}
        Старый статус → {oldStatus.GetDisplayName()}
        Новый статус → {newStatus.GetDisplayName()}
        Технические характеристики:
        {request.TechnicalSpecs}
        Требования:
        {request.Requirements}
        Спасибо за использование системы КИПиА!
        ";

            _ = Task.Run(() => SendNotification(request.Creator.Email, message));

            // Возвращаем упрощенный объект
            return Ok(new
            {
                request.Id,
                request.UniqueNumber,
                CreatedAt = request.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                WorkType = request.WorkType.ToString(),
                Priority = request.Priority.ToString(),
                Status = request.Status.ToString(),
                request.TechnicalSpecs,
                request.Requirements,
                EquipmentInstanceId = request.EquipmentInstanceId,
                TechnicalObjectId = request.TechnicalObjectId,
                WorkshopId = request.WorkshopId,
                TechnologicalUnitId = request.TechnologicalUnitId,
                AssigneeId = request.AssigneeId,
                CreatorId = request.CreatorId,
                // Данные для отображения
                EquipmentInstance = request.EquipmentInstance != null ? new
                {
                    request.EquipmentInstance.Id,
                    Name = request.EquipmentInstance.EquipmentType.Name + " (" +
                           (string.IsNullOrEmpty(request.EquipmentInstance.FactoryNumber) ?
                            request.EquipmentInstance.InventoryNumber :
                            request.EquipmentInstance.FactoryNumber) + ")",
                    EquipmentType = request.EquipmentInstance.EquipmentType.Name
                } : null,
                TechnicalObject = request.TechnicalObject != null ? new
                {
                    request.TechnicalObject.Id,
                    request.TechnicalObject.Name,
                    ComplexName = request.TechnicalObject.Complex?.Name
                } : null,
                Assignee = request.Assignee != null ? new
                {
                    request.Assignee.Id,
                    request.Assignee.Username
                } : null,
                Creator = new
                {
                    request.Creator.Id,
                    request.Creator.Username
                },
                Workshop = request.Workshop != null ? new
                {
                    request.Workshop.Id,
                    Name = request.Workshop.Name + " (" + request.Workshop.Code + ")"
                } : null,
                TechnologicalUnit = request.TechnologicalUnit != null ? new
                {
                    request.TechnologicalUnit.Id,
                    request.TechnologicalUnit.Name
                } : null
            });
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Applicant,Dispatcher,Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = await _context.Requests
                .Include(r => r.EquipmentInstance)
                    .ThenInclude(ei => ei.EquipmentType)
                .Include(r => r.TechnicalObject)
                    .ThenInclude(to => to.Complex)
                .Include(r => r.Workshop)
                .Include(r => r.TechnologicalUnit)
                .Include(r => r.Assignee)
                .Include(r => r.Creator)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            // Проверка прав
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (User.IsInRole("Applicant") && (request.CreatorId != currentUserId || request.Status != RequestStatus.New))
                return Forbid();

            // Проверка связанных сущностей
            var equipmentInstance = await _context.EquipmentInstances
                .Include(ei => ei.EquipmentType)
                .FirstOrDefaultAsync(ei => ei.Id == dto.EquipmentInstanceId);

            if (equipmentInstance == null || equipmentInstance.EquipmentType == null)
                return BadRequest("Invalid EquipmentInstanceId or EquipmentType");

            var technicalObject = await _context.TechnicalObjects
                .Include(to => to.Complex)
                .FirstOrDefaultAsync(to => to.Id == dto.TechnicalObjectId);

            if (technicalObject == null)
                return BadRequest("Invalid TechnicalObjectId");

            var workshop = await _context.Workshops.FindAsync(dto.WorkshopId);
            if (workshop == null)
                return BadRequest("Invalid WorkshopId");

            TechnologicalUnit technologicalUnit = null;
            if (dto.TechnologicalUnitId.HasValue)
            {
                technologicalUnit = await _context.TechnologicalUnits.FindAsync(dto.TechnologicalUnitId.Value);
                if (technologicalUnit == null)
                    return BadRequest("Invalid TechnologicalUnitId");
            }

            // Обновление полей
            request.WorkType = dto.WorkType;
            request.Priority = dto.Priority;
            request.TechnicalSpecs = dto.ProblemDescription;
            request.Requirements = dto.AdditionalComments;
            request.EquipmentInstanceId = dto.EquipmentInstanceId;
            request.EquipmentInstance = equipmentInstance;
            request.TechnicalObjectId = dto.TechnicalObjectId;
            request.TechnicalObject = technicalObject;
            request.WorkshopId = dto.WorkshopId;
            request.Workshop = workshop;
            request.TechnologicalUnitId = dto.TechnologicalUnitId;
            request.TechnologicalUnit = technologicalUnit;

            // Назначение исполнителя — только для диспетчера/админа
            if (User.IsInRole("Dispatcher") || User.IsInRole("Admin"))
            {
                request.AssigneeId = dto.AssigneeId;
            }

            await _context.SaveChangesAsync();

            // Возвращаем упрощенный объект с проверками на null
            return Ok(new
            {
                request.Id,
                request.UniqueNumber,
                CreatedAt = request.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                WorkType = request.WorkType.ToString(),
                Priority = request.Priority.ToString(),
                Status = request.Status.ToString(),
                request.TechnicalSpecs,
                request.Requirements,
                EquipmentInstanceId = request.EquipmentInstanceId,
                TechnicalObjectId = request.TechnicalObjectId,
                WorkshopId = request.WorkshopId,
                TechnologicalUnitId = request.TechnologicalUnitId,
                AssigneeId = request.AssigneeId,
                CreatorId = request.CreatorId,
                // Данные для отображения с проверками на null
                EquipmentInstance = new
                {
                    equipmentInstance.Id,
                    Name = equipmentInstance.EquipmentType != null ?
                           $"{equipmentInstance.EquipmentType.Name} ({(string.IsNullOrEmpty(equipmentInstance.FactoryNumber) ? equipmentInstance.InventoryNumber : equipmentInstance.FactoryNumber)})" :
                           "Неизвестное оборудование",
                    EquipmentType = equipmentInstance.EquipmentType?.Name
                },
                TechnicalObject = new
                {
                    technicalObject.Id,
                    technicalObject.Name,
                    ComplexName = technicalObject.Complex?.Name
                },
                Assignee = request.Assignee != null ? new
                {
                    request.Assignee.Id,
                    request.Assignee.Username
                } : null,
                Creator = new
                {
                    request.Creator.Id,
                    request.Creator.Username
                },
                Workshop = new
                {
                    workshop.Id,
                    Name = $"{workshop.Name} ({workshop.Code})"
                },
                TechnologicalUnit = technologicalUnit != null ? new
                {
                    technologicalUnit.Id,
                    technologicalUnit.Name
                } : null
            });
        }
        // Удаление
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null) return NotFound();
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return Ok();
        }
        // Назначить исполнителя на заявку
        [HttpPut("{id}/assign")]
        [Authorize(Roles = "Dispatcher,Admin")]
        public async Task<IActionResult> AssignExecutor(int id, [FromBody] AssignExecutorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = await _context.Requests
                .Include(r => r.Assignee)
                .Include(r => r.Creator)
                .Include(r => r.EquipmentInstance).ThenInclude(ei => ei.EquipmentType)
                .Include(r => r.TechnicalObject)
                .Include(r => r.Workshop)
                .Include(r => r.TechnologicalUnit)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound("Заявка не найдена");

            var executor = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == dto.AssigneeId && u.Role == UserRole.Executor);

            if (executor == null)
                return BadRequest("Указанный пользователь не найден или не является исполнителем");

            request.AssigneeId = dto.AssigneeId;
            request.Assignee = executor;

            if (request.Status == RequestStatus.New)
                request.Status = RequestStatus.InProgress;

            await _context.SaveChangesAsync();

            // Отправка уведомлений
            var notificationMessage = $@"Вам назначена новая заявка!
        Номер: {request.UniqueNumber}
        Тип работы: {request.WorkType.GetDisplayName()}
        Приоритет: {request.Priority.GetDisplayName()}
        Объект: {request.TechnicalObject?.Name ?? "не указан"}
        Оборудование: {request.EquipmentInstance?.EquipmentType.Name ?? "не указано"}
        Технические характеристики:
        {request.TechnicalSpecs}
        Требования:
        {request.Requirements}
        Зайдите в систему для принятия заявки в работу.
        ";

            _ = Task.Run(() => SendNotification(executor.Email, notificationMessage));

            var creatorMessage = $"На вашу заявку {request.UniqueNumber} назначен исполнитель: {executor.Username}";
            _ = Task.Run(() => SendNotification(request.Creator.Email, creatorMessage));

            // Возвращаем упрощенный объект
            return Ok(new
            {
                request.Id,
                request.UniqueNumber,
                CreatedAt = request.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                WorkType = request.WorkType.ToString(),
                Priority = request.Priority.ToString(),
                Status = request.Status.ToString(),
                request.TechnicalSpecs,
                request.Requirements,
                EquipmentInstanceId = request.EquipmentInstanceId,
                TechnicalObjectId = request.TechnicalObjectId,
                WorkshopId = request.WorkshopId,
                TechnologicalUnitId = request.TechnologicalUnitId,
                AssigneeId = request.AssigneeId,
                CreatorId = request.CreatorId,
                // Данные для отображения
                EquipmentInstance = request.EquipmentInstance != null ? new
                {
                    request.EquipmentInstance.Id,
                    Name = request.EquipmentInstance.EquipmentType.Name + " (" +
                           (string.IsNullOrEmpty(request.EquipmentInstance.FactoryNumber) ?
                            request.EquipmentInstance.InventoryNumber :
                            request.EquipmentInstance.FactoryNumber) + ")",
                    EquipmentType = request.EquipmentInstance.EquipmentType.Name
                } : null,
                TechnicalObject = request.TechnicalObject != null ? new
                {
                    request.TechnicalObject.Id,
                    request.TechnicalObject.Name,
                    ComplexName = request.TechnicalObject.Complex?.Name
                } : null,
                Assignee = new
                {
                    executor.Id,
                    executor.Username
                },
                Creator = new
                {
                    request.Creator.Id,
                    request.Creator.Username
                },
                Workshop = request.Workshop != null ? new
                {
                    request.Workshop.Id,
                    Name = request.Workshop.Name + " (" + request.Workshop.Code + ")"
                } : null,
                TechnologicalUnit = request.TechnologicalUnit != null ? new
                {
                    request.TechnologicalUnit.Id,
                    request.TechnologicalUnit.Name
                } : null
            });
        }
        private async Task SendNotification(string toEmail, string message)
        {
            try
            {
                var emailSettings = _config.GetSection("EmailSettings");
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(MailboxAddress.Parse(emailSettings["SenderEmail"]));
                mimeMessage.To.Add(MailboxAddress.Parse(toEmail));
                mimeMessage.Subject = "КИПиА: Изменение статуса заявки";
                mimeMessage.Body = new TextPart("plain") { Text = message };
                using var client = new SmtpClient();
                await client.ConnectAsync(emailSettings["SmtpServer"], 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(emailSettings["Username"], emailSettings["Password"]);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Логируем, но не падаем
            }
        }
    }

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            return value switch
            {
                RequestStatus.New => "Новая",
                RequestStatus.InProgress => "В работе",
                RequestStatus.Completed => "Выполнена",
                RequestStatus.Rejected => "Отклонена",
                _ => value.ToString()
            };
        }
    }

}
