using kursovou_wed.models;
using Microsoft.EntityFrameworkCore;
using SistemRequestKPU.Models;
using System.Reflection;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Request> Requests { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Workshop> Workshops { get; set; }
    public DbSet<TechnologicalUnit> TechnologicalUnits { get; set; }
    public DbSet<Complex> Complexes { get; set; }
    public DbSet<TechnicalObject> TechnicalObjects { get; set; }
    public DbSet<EquipmentType> EquipmentTypes { get; set; }
    public DbSet<EquipmentInstance> EquipmentInstances { get; set; }
    public DbSet<EquipmentParameter> EquipmentParameters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация связей, индексы и т.д.
        modelBuilder.Entity<Request>()
        .HasOne(r => r.Assignee) // Исполнитель
        .WithMany(u => u.AssignedRequests)
        .HasForeignKey(r => r.AssigneeId);
        
        modelBuilder.Entity<Request>()
        .HasOne(r => r.EquipmentInstance)
        .WithMany()
        .HasForeignKey(r => r.EquipmentInstanceId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Request>()
        .HasOne(r => r.TechnicalObject)
        .WithMany()
        .HasForeignKey(r => r.TechnicalObjectId)
        .OnDelete(DeleteBehavior.Cascade);
       
        modelBuilder.Entity<Workshop>()
        .HasOne(w => w.ResponsiblePerson)
        .WithMany()
        .HasForeignKey(w => w.ResponsiblePersonId)
        .OnDelete(DeleteBehavior.SetNull); 

        modelBuilder.Entity<TechnologicalUnit>()
        .HasOne(t => t.TechnicalObject)
        .WithMany(to => to.TechnologicalUnits)
        .HasForeignKey(t => t.TechnicalObjectId)
        .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<TechnologicalUnit>()
        .HasOne(t => t.Workshop)
        .WithMany(w => w.TechnologicalUnits)
        .HasForeignKey(t => t.WorkshopId)
        .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<TechnicalObject>()
        .HasOne(t => t.Complex)
        .WithMany(c => c.TechnicalObjects)
        .HasForeignKey(t => t.ComplexId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EquipmentInstance>()
        .HasOne(e => e.EquipmentType)
        .WithMany(et => et.Instances)
        .HasForeignKey(e => e.EquipmentTypeId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EquipmentInstance>()
        .HasOne(e => e.TechnicalObject)
        .WithMany(to => to.EquipmentInstances)
        .HasForeignKey(e => e.TechnicalObjectId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EquipmentInstance>()
        .HasOne(e => e.TechnologicalUnit)
        .WithMany(tu => tu.EquipmentInstances)
        .HasForeignKey(e => e.TechnologicalUnitId)
        .OnDelete(DeleteBehavior.SetNull);
      
        modelBuilder.Entity<EquipmentParameter>()
        .HasOne(ep => ep.EquipmentInstance)
        .WithMany(ei => ei.Parameters)
        .HasForeignKey(ep => ep.EquipmentInstanceId)
        .OnDelete(DeleteBehavior.Cascade);

        // 1. Создаем пользователей с разными ролями
        var adminUser = new User
        {
            Id = 1,
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
            Email = "admin@example.com",
            Role = UserRole.Admin
        };

        var dispatcherUser = new User
        {
            Id = 2,
            Username = "dispatcher",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Disp123"),
            Email = "dispatcher@example.com",
            Role = UserRole.Dispatcher
        };

        var executorUser = new User
        {
            Id = 3,
            Username = "executor1",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Exec123"),
            Email = "executor1@example.com",
            Role = UserRole.Executor
        };

        var executorUser2 = new User
        {
            Id = 4,
            Username = "executor2",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Exec123"),
            Email = "executor2@example.com",
            Role = UserRole.Executor
        };

        var applicantUser = new User
        {
            Id = 5,
            Username = "applicant",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Applicant123"),
            Email = "applicant@example.com",
            Role = UserRole.Applicant
        };

        modelBuilder.Entity<User>().HasData(
            adminUser,
            dispatcherUser,
            executorUser,
            executorUser2,
            applicantUser
        );

        // 2. Complex
        modelBuilder.Entity<Complex>().HasData(
            new Complex { Id = 1, Name = "Газокомпрессорная станция", Type = "ГКС", Location = "Площадка ГКС" },
            new Complex { Id = 2, Name = "Газораспределительная станция", Type = "ГРС", Location = "Площадка ГРС" }
        );

        // 3. TechnicalObject (Объект)
        modelBuilder.Entity<TechnicalObject>().HasData(
            new TechnicalObject { Id = 1, Name = "Газокомпрессорная станция", ComplexId = 1, ObjectType = "ГКС" },
            new TechnicalObject { Id = 2, Name = "Газораспределительная станция", ComplexId = 2, ObjectType = "ГРС" }
        );

        // 4. Workshop (Цех)
        modelBuilder.Entity<Workshop>().HasData(
            new Workshop { Id = 1, Name = "Компрессорный цех №1", Code = "КЦ1", ResponsiblePersonId = 1 },
            new Workshop { Id = 2, Name = "Компрессорный цех №2", Code = "КЦ2", ResponsiblePersonId = 1 },
            new Workshop { Id = 3, Name = "Компрессорный цех №3", Code = "КЦ3", ResponsiblePersonId = 1 },
            new Workshop { Id = 4, Name = "ГРС №1", Code = "ГРС1", ResponsiblePersonId = 1 },
            new Workshop { Id = 5, Name = "ГРС №2", Code = "ГРС2", ResponsiblePersonId = 1 },
            new Workshop { Id = 6, Name = "ГРС №3", Code = "ГРС3", ResponsiblePersonId = 1 }
        );

        // 5. TechnologicalUnit (Техническое устройство)
        modelBuilder.Entity<TechnologicalUnit>().HasData(
            // ГКС
            new TechnologicalUnit { Id = 1, Name = "Газоперекачивающий агрегат №1.1", Code = "ГПА-1.1", WorkshopId = 1, TechnicalObjectId = 1 },
            new TechnologicalUnit { Id = 2, Name = "Газоперекачивающий агрегат №1.2", Code = "ГПА-1.2", WorkshopId = 1, TechnicalObjectId = 1 },
            new TechnologicalUnit { Id = 3, Name = "Газоперекачивающий агрегат №2.1", Code = "ГПА-2.1", WorkshopId = 2, TechnicalObjectId = 1 },
            new TechnologicalUnit { Id = 4, Name = "Газоперекачивающий агрегат №2.2", Code = "ГПА-2.2", WorkshopId = 2, TechnicalObjectId = 1 },
            new TechnologicalUnit { Id = 5, Name = "Газоперекачивающий агрегат №3.1", Code = "ГПА-3.1", WorkshopId = 3, TechnicalObjectId = 1 },
            new TechnologicalUnit { Id = 6, Name = "Газоперекачивающий агрегат №3.2", Code = "ГПА-3.2", WorkshopId = 3, TechnicalObjectId = 1 },

            // ГРС №1
            new TechnologicalUnit { Id = 7, Name = "Узел очистки газа", Code = "УОГ-1", WorkshopId = 4, TechnicalObjectId = 2 },
            new TechnologicalUnit { Id = 8, Name = "Узел редуцирования газа", Code = "УРГ-1", WorkshopId = 4, TechnicalObjectId = 2 },
            new TechnologicalUnit { Id = 9, Name = "Узел подогрева и одоризации газа", Code = "УПОГ-1", WorkshopId = 4, TechnicalObjectId = 2 },

            // ГРС №2
            new TechnologicalUnit { Id = 10, Name = "Узел очистки газа", Code = "УОГ-2", WorkshopId = 5, TechnicalObjectId = 2 },
            new TechnologicalUnit { Id = 11, Name = "Узел редуцирования газа", Code = "УРГ-2", WorkshopId = 5, TechnicalObjectId = 2 },
            new TechnologicalUnit { Id = 12, Name = "Узел подогрева и одоризации газа", Code = "УПОГ-2", WorkshopId = 5, TechnicalObjectId = 2 },

            // ГРС №3
            new TechnologicalUnit { Id = 13, Name = "Узел очистки газа", Code = "УОГ-3", WorkshopId = 6, TechnicalObjectId = 2 },
            new TechnologicalUnit { Id = 14, Name = "Узел редуцирования газа", Code = "УРГ-3", WorkshopId = 6, TechnicalObjectId = 2 },
            new TechnologicalUnit { Id = 15, Name = "Узел подогрева и одоризации газа", Code = "УПОГ-3", WorkshopId = 6, TechnicalObjectId = 2 }
        );

        // 6. EquipmentType (типы оборудования)
        modelBuilder.Entity<EquipmentType>().HasData(
            new EquipmentType { Id = 1, Name = "Главный контроллер управления", Manufacturer = "KPU", Model = "CTRL-MAIN", Specifications = "Контроллер" },
            new EquipmentType { Id = 2, Name = "Система автоматического управления", Manufacturer = "KPU", Model = "ASU", Specifications = "АСУ" },
            new EquipmentType { Id = 3, Name = "Датчик оборотов двигателя основной турбины", Manufacturer = "KPU", Model = "RPM-TURB", Specifications = "Датчик оборотов" },
            new EquipmentType { Id = 4, Name = "Датчик температуры основной турбины", Manufacturer = "KPU", Model = "TEMP-TURB", Specifications = "Датчик температуры" },
            new EquipmentType { Id = 5, Name = "Датчик оборотов нагнетателя", Manufacturer = "KPU", Model = "RPM-COMP", Specifications = "Датчик оборотов" },
            new EquipmentType { Id = 6, Name = "Вспомогательное оборудование", Manufacturer = "KPU", Model = "AUX", Specifications = "Вспомогательное" },
            new EquipmentType { Id = 7, Name = "Датчик давления основного фильтра", Manufacturer = "KPU", Model = "PRESS-FILTER", Specifications = "Датчик давления" },
            new EquipmentType { Id = 8, Name = "Датчик давления на входе", Manufacturer = "KPU", Model = "PRESS-IN", Specifications = "Датчик давления" },
            new EquipmentType { Id = 9, Name = "Датчик температуры на входе", Manufacturer = "KPU", Model = "TEMP-IN", Specifications = "Датчик температуры" },
            new EquipmentType { Id = 10, Name = "Датчик регулятора давления", Manufacturer = "KPU", Model = "PRESS-REG", Specifications = "Датчик давления" },
            new EquipmentType { Id = 11, Name = "Датчик вибрации регулятора", Manufacturer = "KPU", Model = "VIB-REG", Specifications = "Датчик вибрации" },
            new EquipmentType { Id = 12, Name = "Датчик температуры регулятора", Manufacturer = "KPU", Model = "TEMP-REG", Specifications = "Датчик температуры" },
            new EquipmentType { Id = 13, Name = "Контроллер платы управления", Manufacturer = "KPU", Model = "CTRL-BOARD", Specifications = "Контроллер" },
            new EquipmentType { Id = 14, Name = "Датчик температуры подогревателя", Manufacturer = "KPU", Model = "TEMP-HEATER", Specifications = "Датчик температуры" },
            new EquipmentType { Id = 15, Name = "Датчик наличия пламени", Manufacturer = "KPU", Model = "FLAME", Specifications = "Датчик пламени" },
            new EquipmentType { Id = 16, Name = "Датчик уровня расхода одоранта", Manufacturer = "KPU", Model = "ODOR-LEVEL", Specifications = "Датчик уровня" }
        );

        // 7. EquipmentInstance (конкретные экземпляры оборудования)
        modelBuilder.Entity<EquipmentInstance>().HasData(
            // ГПА №1.1
            new EquipmentInstance { Id = 1, EquipmentTypeId = 1, TechnicalObjectId = 1, TechnologicalUnitId = 1, InventoryNumber = "ГПА1.1-001" },
            new EquipmentInstance { Id = 2, EquipmentTypeId = 2, TechnicalObjectId = 1, TechnologicalUnitId = 1, InventoryNumber = "ГПА1.1-002" },
            new EquipmentInstance { Id = 3, EquipmentTypeId = 3, TechnicalObjectId = 1, TechnologicalUnitId = 1, InventoryNumber = "ГПА1.1-003" },
            new EquipmentInstance { Id = 4, EquipmentTypeId = 4, TechnicalObjectId = 1, TechnologicalUnitId = 1, InventoryNumber = "ГПА1.1-004" },
            new EquipmentInstance { Id = 5, EquipmentTypeId = 5, TechnicalObjectId = 1, TechnologicalUnitId = 1, InventoryNumber = "ГПА1.1-005" },
            new EquipmentInstance { Id = 6, EquipmentTypeId = 6, TechnicalObjectId = 1, TechnologicalUnitId = 1, InventoryNumber = "ГПА1.1-006" },

            // ГПА №1.2
            new EquipmentInstance { Id = 7, EquipmentTypeId = 1, TechnicalObjectId = 1, TechnologicalUnitId = 2, InventoryNumber = "ГПА1.2-001" },
            new EquipmentInstance { Id = 8, EquipmentTypeId = 2, TechnicalObjectId = 1, TechnologicalUnitId = 2, InventoryNumber = "ГПА1.2-002" },
            new EquipmentInstance { Id = 9, EquipmentTypeId = 3, TechnicalObjectId = 1, TechnologicalUnitId = 2, InventoryNumber = "ГПА1.2-003" },
            new EquipmentInstance { Id = 10, EquipmentTypeId = 4, TechnicalObjectId = 1, TechnologicalUnitId = 2, InventoryNumber = "ГПА1.2-004" },
            new EquipmentInstance { Id = 11, EquipmentTypeId = 5, TechnicalObjectId = 1, TechnologicalUnitId = 2, InventoryNumber = "ГПА1.2-005" },
            new EquipmentInstance { Id = 12, EquipmentTypeId = 6, TechnicalObjectId = 1, TechnologicalUnitId = 2, InventoryNumber = "ГПА1.2-006" },

            // ГПА №2.1
            new EquipmentInstance { Id = 13, EquipmentTypeId = 1, TechnicalObjectId = 1, TechnologicalUnitId = 3, InventoryNumber = "ГПА2.1-001" },
            new EquipmentInstance { Id = 14, EquipmentTypeId = 2, TechnicalObjectId = 1, TechnologicalUnitId = 3, InventoryNumber = "ГПА2.1-002" },
            new EquipmentInstance { Id = 15, EquipmentTypeId = 3, TechnicalObjectId = 1, TechnologicalUnitId = 3, InventoryNumber = "ГПА2.1-003" },
            new EquipmentInstance { Id = 16, EquipmentTypeId = 4, TechnicalObjectId = 1, TechnologicalUnitId = 3, InventoryNumber = "ГПА2.1-004" },
            new EquipmentInstance { Id = 17, EquipmentTypeId = 5, TechnicalObjectId = 1, TechnologicalUnitId = 3, InventoryNumber = "ГПА2.1-005" },
            new EquipmentInstance { Id = 18, EquipmentTypeId = 6, TechnicalObjectId = 1, TechnologicalUnitId = 3, InventoryNumber = "ГПА2.1-006" },

            // ГПА №2.2
            new EquipmentInstance { Id = 19, EquipmentTypeId = 1, TechnicalObjectId = 1, TechnologicalUnitId = 4, InventoryNumber = "ГПА2.2-001" },
            new EquipmentInstance { Id = 20, EquipmentTypeId = 2, TechnicalObjectId = 1, TechnologicalUnitId = 4, InventoryNumber = "ГПА2.2-002" },
            new EquipmentInstance { Id = 21, EquipmentTypeId = 3, TechnicalObjectId = 1, TechnologicalUnitId = 4, InventoryNumber = "ГПА2.2-003" },
            new EquipmentInstance { Id = 22, EquipmentTypeId = 4, TechnicalObjectId = 1, TechnologicalUnitId = 4, InventoryNumber = "ГПА2.2-004" },
            new EquipmentInstance { Id = 23, EquipmentTypeId = 5, TechnicalObjectId = 1, TechnologicalUnitId = 4, InventoryNumber = "ГПА2.2-005" },
            new EquipmentInstance { Id = 24, EquipmentTypeId = 6, TechnicalObjectId = 1, TechnologicalUnitId = 4, InventoryNumber = "ГПА2.2-006" },

            // ГПА №3.1
            new EquipmentInstance { Id = 25, EquipmentTypeId = 1, TechnicalObjectId = 1, TechnologicalUnitId = 5, InventoryNumber = "ГПА3.1-001" },
            new EquipmentInstance { Id = 26, EquipmentTypeId = 2, TechnicalObjectId = 1, TechnologicalUnitId = 5, InventoryNumber = "ГПА3.1-002" },
            new EquipmentInstance { Id = 27, EquipmentTypeId = 3, TechnicalObjectId = 1, TechnologicalUnitId = 5, InventoryNumber = "ГПА3.1-003" },
            new EquipmentInstance { Id = 28, EquipmentTypeId = 4, TechnicalObjectId = 1, TechnologicalUnitId = 5, InventoryNumber = "ГПА3.1-004" },
            new EquipmentInstance { Id = 29, EquipmentTypeId = 5, TechnicalObjectId = 1, TechnologicalUnitId = 5, InventoryNumber = "ГПА3.1-005" },
            new EquipmentInstance { Id = 30, EquipmentTypeId = 6, TechnicalObjectId = 1, TechnologicalUnitId = 5, InventoryNumber = "ГПА3.1-006" },

            // ГПА №3.2
            new EquipmentInstance { Id = 31, EquipmentTypeId = 1, TechnicalObjectId = 1, TechnologicalUnitId = 6, InventoryNumber = "ГПА3.2-001" },
            new EquipmentInstance { Id = 32, EquipmentTypeId = 2, TechnicalObjectId = 1, TechnologicalUnitId = 6, InventoryNumber = "ГПА3.2-002" },
            new EquipmentInstance { Id = 33, EquipmentTypeId = 3, TechnicalObjectId = 1, TechnologicalUnitId = 6, InventoryNumber = "ГПА3.2-003" },
            new EquipmentInstance { Id = 34, EquipmentTypeId = 4, TechnicalObjectId = 1, TechnologicalUnitId = 6, InventoryNumber = "ГПА3.2-004" },
            new EquipmentInstance { Id = 35, EquipmentTypeId = 5, TechnicalObjectId = 1, TechnologicalUnitId = 6, InventoryNumber = "ГПА3.2-005" },
            new EquipmentInstance { Id = 36, EquipmentTypeId = 6, TechnicalObjectId = 1, TechnologicalUnitId = 6, InventoryNumber = "ГПА3.2-006" },

            // ГРС №1
            new EquipmentInstance { Id = 37, EquipmentTypeId = 7, TechnicalObjectId = 2, TechnologicalUnitId = 7, InventoryNumber = "ГРС1-УОГ-001" },
            new EquipmentInstance { Id = 38, EquipmentTypeId = 8, TechnicalObjectId = 2, TechnologicalUnitId = 7, InventoryNumber = "ГРС1-УОГ-002" },
            new EquipmentInstance { Id = 39, EquipmentTypeId = 9, TechnicalObjectId = 2, TechnologicalUnitId = 7, InventoryNumber = "ГРС1-УОГ-003" },
            new EquipmentInstance { Id = 40, EquipmentTypeId = 10, TechnicalObjectId = 2, TechnologicalUnitId = 8, InventoryNumber = "ГРС1-УРГ-001" },
            new EquipmentInstance { Id = 41, EquipmentTypeId = 11, TechnicalObjectId = 2, TechnologicalUnitId = 8, InventoryNumber = "ГРС1-УРГ-002" },
            new EquipmentInstance { Id = 42, EquipmentTypeId = 12, TechnicalObjectId = 2, TechnologicalUnitId = 8, InventoryNumber = "ГРС1-УРГ-003" },
            new EquipmentInstance { Id = 43, EquipmentTypeId = 13, TechnicalObjectId = 2, TechnologicalUnitId = 8, InventoryNumber = "ГРС1-УРГ-004" },
            new EquipmentInstance { Id = 44, EquipmentTypeId = 14, TechnicalObjectId = 2, TechnologicalUnitId = 9, InventoryNumber = "ГРС1-УПОГ-001" },
            new EquipmentInstance { Id = 45, EquipmentTypeId = 15, TechnicalObjectId = 2, TechnologicalUnitId = 9, InventoryNumber = "ГРС1-УПОГ-002" },
            new EquipmentInstance { Id = 46, EquipmentTypeId = 16, TechnicalObjectId = 2, TechnologicalUnitId = 9, InventoryNumber = "ГРС1-УПОГ-003" },
            new EquipmentInstance { Id = 47, EquipmentTypeId = 13, TechnicalObjectId = 2, TechnologicalUnitId = 9, InventoryNumber = "ГРС1-УПОГ-004" },

            // ГРС №2
            new EquipmentInstance { Id = 48, EquipmentTypeId = 7, TechnicalObjectId = 2, TechnologicalUnitId = 10, InventoryNumber = "ГРС2-УОГ-001" },
            new EquipmentInstance { Id = 49, EquipmentTypeId = 8, TechnicalObjectId = 2, TechnologicalUnitId = 10, InventoryNumber = "ГРС2-УОГ-002" },
            new EquipmentInstance { Id = 50, EquipmentTypeId = 9, TechnicalObjectId = 2, TechnologicalUnitId = 10, InventoryNumber = "ГРС2-УОГ-003" },
            new EquipmentInstance { Id = 51, EquipmentTypeId = 10, TechnicalObjectId = 2, TechnologicalUnitId = 11, InventoryNumber = "ГРС2-УРГ-001" },
            new EquipmentInstance { Id = 52, EquipmentTypeId = 11, TechnicalObjectId = 2, TechnologicalUnitId = 11, InventoryNumber = "ГРС2-УРГ-002" },
            new EquipmentInstance { Id = 53, EquipmentTypeId = 12, TechnicalObjectId = 2, TechnologicalUnitId = 11, InventoryNumber = "ГРС2-УРГ-003" },
            new EquipmentInstance { Id = 54, EquipmentTypeId = 13, TechnicalObjectId = 2, TechnologicalUnitId = 11, InventoryNumber = "ГРС2-УРГ-004" },
            new EquipmentInstance { Id = 55, EquipmentTypeId = 14, TechnicalObjectId = 2, TechnologicalUnitId = 12, InventoryNumber = "ГРС2-УПОГ-001" },
            new EquipmentInstance { Id = 56, EquipmentTypeId = 15, TechnicalObjectId = 2, TechnologicalUnitId = 12, InventoryNumber = "ГРС2-УПОГ-002" },
            new EquipmentInstance { Id = 57, EquipmentTypeId = 16, TechnicalObjectId = 2, TechnologicalUnitId = 12, InventoryNumber = "ГРС2-УПОГ-003" },
            new EquipmentInstance { Id = 58, EquipmentTypeId = 13, TechnicalObjectId = 2, TechnologicalUnitId = 12, InventoryNumber = "ГРС2-УПОГ-004" },

            // ГРС №3
            new EquipmentInstance { Id = 59, EquipmentTypeId = 7, TechnicalObjectId = 2, TechnologicalUnitId = 13, InventoryNumber = "ГРС3-УОГ-001" },
            new EquipmentInstance { Id = 60, EquipmentTypeId = 8, TechnicalObjectId = 2, TechnologicalUnitId = 13, InventoryNumber = "ГРС3-УОГ-002" },
            new EquipmentInstance { Id = 61, EquipmentTypeId = 9, TechnicalObjectId = 2, TechnologicalUnitId = 13, InventoryNumber = "ГРС3-УОГ-003" },
            new EquipmentInstance { Id = 62, EquipmentTypeId = 10, TechnicalObjectId = 2, TechnologicalUnitId = 14, InventoryNumber = "ГРС3-УРГ-001" },
            new EquipmentInstance { Id = 63, EquipmentTypeId = 11, TechnicalObjectId = 2, TechnologicalUnitId = 14, InventoryNumber = "ГРС3-УРГ-002" },
            new EquipmentInstance { Id = 64, EquipmentTypeId = 12, TechnicalObjectId = 2, TechnologicalUnitId = 14, InventoryNumber = "ГРС3-УРГ-003" },
            new EquipmentInstance { Id = 65, EquipmentTypeId = 13, TechnicalObjectId = 2, TechnologicalUnitId = 14, InventoryNumber = "ГРС3-УРГ-004" },
            new EquipmentInstance { Id = 66, EquipmentTypeId = 14, TechnicalObjectId = 2, TechnologicalUnitId = 15, InventoryNumber = "ГРС3-УПОГ-001" },
            new EquipmentInstance { Id = 67, EquipmentTypeId = 15, TechnicalObjectId = 2, TechnologicalUnitId = 15, InventoryNumber = "ГРС3-УПОГ-002" },
            new EquipmentInstance { Id = 68, EquipmentTypeId = 16, TechnicalObjectId = 2, TechnologicalUnitId = 15, InventoryNumber = "ГРС3-УПОГ-003" },
            new EquipmentInstance { Id = 69, EquipmentTypeId = 13, TechnicalObjectId = 2, TechnologicalUnitId = 15, InventoryNumber = "ГРС3-УПОГ-004" }
        );
    }
}
