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
        // Обновите связи в Request: теперь ссылка на EquipmentInstance вместо Equipment
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
        // Связи для новых моделей
        modelBuilder.Entity<Workshop>()
        .HasOne(w => w.ResponsiblePerson)
        .WithMany()
        .HasForeignKey(w => w.ResponsiblePersonId)
        .OnDelete(DeleteBehavior.SetNull); // Optional, чтобы не удалять пользователя

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
        .OnDelete(DeleteBehavior.SetNull); // Optional
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

        // Seed для TechnicalObject
        modelBuilder.Entity<TechnicalObject>().HasData(
            new TechnicalObject { Id = 1, Name = "Газокомпрессорная станция", ComplexId = 1, ObjectType = "ГКС", IsGRS = false },
            new TechnicalObject { Id = 2, Name = "Газораспределительная станция", ComplexId = 2, ObjectType = "ГРС", IsGRS = true }
        );

        // Seed для Workshop
        modelBuilder.Entity<Workshop>().HasData(
            new Workshop { Id = 1, Name = "Компрессорный цех №1", Code = "КЦ1", ResponsiblePersonId = 1 },
            new Workshop { Id = 2, Name = "Компрессорный цех №2", Code = "КЦ2", ResponsiblePersonId = 1 },
            new Workshop { Id = 3, Name = "Компрессорный цех №3", Code = "КЦ3", ResponsiblePersonId = 1 },
            new Workshop { Id = 4, Name = "Распределительный цех №1", Code = "РЦ1", ResponsiblePersonId = 1 }
        );

        // Seed для TechnologicalUnit
        modelBuilder.Entity<TechnologicalUnit>().HasData(
            // ГКС
            new TechnologicalUnit { Id = 1, Name = "Газоперекачивающий агрегат №1.1", Code = "ГПА-1.1", WorkshopId = 1, TechnicalObjectId = 1 },
            new TechnologicalUnit { Id = 2, Name = "Газоперекачивающий агрегат №1.2", Code = "ГПА-1.2", WorkshopId = 1, TechnicalObjectId = 1 },
            new TechnologicalUnit { Id = 3, Name = "Газоперекачивающий агрегат №2.1", Code = "ГПА-2.1", WorkshopId = 2, TechnicalObjectId = 1 },
            new TechnologicalUnit { Id = 4, Name = "Газоперекачивающий агрегат №2.2", Code = "ГПА-2.2", WorkshopId = 2, TechnicalObjectId = 1 },
            new TechnologicalUnit { Id = 5, Name = "Газоперекачивающий агрегат №3.1", Code = "ГПА-3.1", WorkshopId = 3, TechnicalObjectId = 1 },
            new TechnologicalUnit { Id = 6, Name = "Газоперекачивающий агрегат №3.2", Code = "ГПА-3.2", WorkshopId = 3, TechnicalObjectId = 1 },

            // ГРС
            new TechnologicalUnit { Id = 7, Name = "Узел очистки газа", Code = "УОГ", WorkshopId = 4, TechnicalObjectId = 2 },
            new TechnologicalUnit { Id = 8, Name = "Узел редуцирования газа", Code = "УРГ", WorkshopId = 4, TechnicalObjectId = 2 },
            new TechnologicalUnit { Id = 9, Name = "Узел подогрева и одоризации газа", Code = "УПОГ", WorkshopId = 4, TechnicalObjectId = 2 }
        );

        // Seed для EquipmentInstance (примеры)
        modelBuilder.Entity<EquipmentInstance>().HasData(
            new EquipmentInstance { Id = 1, EquipmentTypeId = 1, TechnicalObjectId = 1, TechnologicalUnitId = 1, InventoryNumber = "ГПА1.1-КУ-001" },
            new EquipmentInstance { Id = 2, EquipmentTypeId = 2, TechnicalObjectId = 1, TechnologicalUnitId = 1, InventoryNumber = "ГПА1.1-АСУ-001" },
            new EquipmentInstance { Id = 3, EquipmentTypeId = 3, TechnicalObjectId = 1, TechnologicalUnitId = 1, InventoryNumber = "ГПА1.1-ДОД-001" }
        );
    }
}
