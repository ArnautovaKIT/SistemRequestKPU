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

        // 2. Создаем комплексы
        var complex1 = new Complex
        {
            Id = 1,
            Name = "Газоперерабатывающий комплекс",
            Type = "ГПЗ",
            Location = "г. Москва, ул. Промышленная, д. 10"
        };

        var complex2 = new Complex
        {
            Id = 2,
            Name = "Нефтеперерабатывающий комплекс",
            Type = "НПЗ",
            Location = "г. Санкт-Петербург, пр. Энергетиков, д. 5"
        };

        modelBuilder.Entity<Complex>().HasData(
            complex1,
            complex2
        );

        // 3. Создаем цеха с ответственными лицами
        var workshop1 = new Workshop
        {
            Id = 1,
            Name = "Цех автоматизации",
            Code = "CA-1",
            ResponsiblePersonId = 2 // dispatcherUser
        };

        var workshop2 = new Workshop
        {
            Id = 2,
            Name = "Цех КИПиА",
            Code = "KIP-2",
            ResponsiblePersonId = 3 // executorUser
        };

        var workshop3 = new Workshop
        {
            Id = 3,
            Name = "Главный производственный цех",
            Code = "GP-3",
            ResponsiblePersonId = 4 // executorUser2
        };

        modelBuilder.Entity<Workshop>().HasData(
            workshop1,
            workshop2,
            workshop3
        );

        // 4. Создаем технологические узлы
        var techUnit1 = new TechnologicalUnit
        {
            Id = 1,
            Name = "Газораспределительная станция",
            Code = "GRS-1",
            WorkshopId = 1,
            Description = "Основная газораспределительная станция комплекса"
        };

        var techUnit2 = new TechnologicalUnit
        {
            Id = 2,
            Name = "Компрессорная установка",
            Code = "KU-2",
            WorkshopId = 1,
            Description = "Компрессорная установка для повышения давления"
        };

        var techUnit3 = new TechnologicalUnit
        {
            Id = 3,
            Name = "Система контроля давления",
            Code = "SKD-3",
            WorkshopId = 2,
            Description = "Система контроля и регулирования давления газа"
        };

        var techUnit4 = new TechnologicalUnit
        {
            Id = 4,
            Name = "Система измерения расхода",
            Code = "SIM-4",
            WorkshopId = 2,
            Description = "Система измерения расхода газа на выходе"
        };

        var techUnit5 = new TechnologicalUnit
        {
            Id = 5,
            Name = "Резервуарный парк",
            Code = "RP-5",
            WorkshopId = 3,
            Description = "Резервуары для хранения нефтепродуктов"
        };

        modelBuilder.Entity<TechnologicalUnit>().HasData(
            techUnit1,
            techUnit2,
            techUnit3,
            techUnit4,
            techUnit5
        );

        // 5. Создаем технические объекты
        var techObject1 = new TechnicalObject
        {
            Id = 1,
            Name = "ГРС-1",
            ComplexId = 1,
            ObjectType = "Газораспределительная станция",
            InstallationDate = new DateTime(2020, 1, 15, 0, 0, 0, DateTimeKind.Utc)
        };

        var techObject2 = new TechnicalObject
        {
            Id = 2,
            Name = "КС-5",
            ComplexId = 1,
            ObjectType = "Компрессорная станция",
            InstallationDate = new DateTime(2018, 5, 20, 0, 0, 0, DateTimeKind.Utc)
        };

        var techObject3 = new TechnicalObject
        {
            Id = 3,
            Name = "Газопровод-10",
            ComplexId = 1,
            ObjectType = "Газопровод",
            InstallationDate = new DateTime(2019, 3, 10, 0, 0, 0, DateTimeKind.Utc)
        };

        var techObject4 = new TechnicalObject
        {
            Id = 4,
            Name = "Резервуар-1",
            ComplexId = 2,
            ObjectType = "Резервуар",
            InstallationDate = new DateTime(2021, 7, 5, 0, 0, 0, DateTimeKind.Utc)
        };

        var techObject5 = new TechnicalObject
        {
            Id = 5,
            Name = "Резервуар-2",
            ComplexId = 2,
            ObjectType = "Резервуар",
            InstallationDate = new DateTime(2021, 7, 5, 0, 0, 0, DateTimeKind.Utc)
        };

        var techObject6 = new TechnicalObject
        {
            Id = 6,
            Name = "Насосная станция",
            ComplexId = 2,
            ObjectType = "Насосная станция",
            InstallationDate = new DateTime(2019, 11, 22, 0, 0, 0, DateTimeKind.Utc)
        };

        modelBuilder.Entity<TechnicalObject>().HasData(
            techObject1,
            techObject2,
            techObject3,
            techObject4,
            techObject5,
            techObject6
        );

        // 6. Создаем типы оборудования
        var equipmentType1 = new EquipmentType
        {
            Id = 1,
            Name = "Датчик давления",
            Manufacturer = "Siemens",
            Model = "МП-100",
            Specifications = "Диапазон 0-100 бар, класс точности 0.5"
        };

        var equipmentType2 = new EquipmentType
        {
            Id = 2,
            Name = "Контроллер автоматики",
            Manufacturer = "ABB",
            Model = "AC800",
            Specifications = "Управление КИПиА, 16 каналов ввода-вывода"
        };

        var equipmentType3 = new EquipmentType
        {
            Id = 3,
            Name = "Прибор измерения расхода",
            Manufacturer = "Emerson",
            Model = "Rosemount 8600",
            Specifications = "Контроль расхода газа, диапазон 0-5000 м³/ч"
        };

        var equipmentType4 = new EquipmentType
        {
            Id = 4,
            Name = "Датчик температуры",
            Manufacturer = "Yokogawa",
            Model = "YT-500",
            Specifications = "Диапазон -50°C до +300°C, класс точности 0.2"
        };

        var equipmentType5 = new EquipmentType
        {
            Id = 5,
            Name = "Электромагнитный клапан",
            Manufacturer = "Festo",
            Model = "MPYE-5-1/8",
            Specifications = "Управление потоком, давление до 10 бар"
        };

        modelBuilder.Entity<EquipmentType>().HasData(
            equipmentType1,
            equipmentType2,
            equipmentType3,
            equipmentType4,
            equipmentType5
        );

        // 7. Создаем экземпляры оборудования
        var equipmentInstance1 = new EquipmentInstance
        {
            Id = 1,
            EquipmentTypeId = 1,
            InventoryNumber = "INV-001",
            FactoryNumber = "FAC-1001",
            StationNumber = "ST-001",
            TechnicalNumber = "TECH-001",
            InstallationDate = new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc),
            TechnicalObjectId = 1,
            CurrentStatus = "В работе",
            LastMaintenanceDate = new DateTime(2023, 1, 15, 0, 0, 0, DateTimeKind.Utc),
            NextMaintenanceDate = new DateTime(2023, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            TechnologicalUnitId = 1
        };

        var equipmentInstance2 = new EquipmentInstance
        {
            Id = 2,
            EquipmentTypeId = 2,
            InventoryNumber = "INV-002",
            FactoryNumber = "FAC-1002",
            StationNumber = "ST-002",
            TechnicalNumber = "TECH-002",
            InstallationDate = new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc),
            TechnicalObjectId = 1,
            CurrentStatus = "В работе",
            LastMaintenanceDate = new DateTime(2023, 1, 20, 0, 0, 0, DateTimeKind.Utc),
            NextMaintenanceDate = new DateTime(2023, 7, 20, 0, 0, 0, DateTimeKind.Utc),
            TechnologicalUnitId = 1
        };

        var equipmentInstance3 = new EquipmentInstance
        {
            Id = 3,
            EquipmentTypeId = 3,
            InventoryNumber = "INV-003",
            FactoryNumber = "FAC-1003",
            StationNumber = "ST-003",
            TechnicalNumber = "TECH-003",
            InstallationDate = new DateTime(2018, 6, 10, 0, 0, 0, DateTimeKind.Utc),
            TechnicalObjectId = 2,
            CurrentStatus = "Требует обслуживания",
            LastMaintenanceDate = new DateTime(2022, 12, 5, 0, 0, 0, DateTimeKind.Utc),
            NextMaintenanceDate = new DateTime(2023, 6, 5, 0, 0, 0, DateTimeKind.Utc),
            TechnologicalUnitId = 2
        };

        var equipmentInstance4 = new EquipmentInstance
        {
            Id = 4,
            EquipmentTypeId = 4,
            InventoryNumber = "INV-004",
            FactoryNumber = "FAC-1004",
            StationNumber = "ST-004",
            TechnicalNumber = "TECH-004",
            InstallationDate = new DateTime(2019, 4, 1, 0, 0, 0, DateTimeKind.Utc),
            TechnicalObjectId = 3,
            CurrentStatus = "В работе",
            LastMaintenanceDate = new DateTime(2023, 2, 10, 0, 0, 0, DateTimeKind.Utc),
            NextMaintenanceDate = new DateTime(2023, 8, 10, 0, 0, 0, DateTimeKind.Utc),
            TechnologicalUnitId = null
        };

        var equipmentInstance5 = new EquipmentInstance
        {
            Id = 5,
            EquipmentTypeId = 5,
            InventoryNumber = "INV-005",
            FactoryNumber = "FAC-1005",
            StationNumber = "ST-005",
            TechnicalNumber = "TECH-005",
            InstallationDate = new DateTime(2021, 8, 15, 0, 0, 0, DateTimeKind.Utc),
            TechnicalObjectId = 4,
            CurrentStatus = "В работе",
            LastMaintenanceDate = new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc),
            NextMaintenanceDate = new DateTime(2023, 9, 1, 0, 0, 0, DateTimeKind.Utc),
            TechnologicalUnitId = 5
        };

        var equipmentInstance6 = new EquipmentInstance
        {
            Id = 6,
            EquipmentTypeId = 1,
            InventoryNumber = "INV-006",
            FactoryNumber = "FAC-1006",
            StationNumber = "ST-006",
            TechnicalNumber = "TECH-006",
            InstallationDate = new DateTime(2021, 8, 15, 0, 0, 0, DateTimeKind.Utc),
            TechnicalObjectId = 5,
            CurrentStatus = "В работе",
            LastMaintenanceDate = new DateTime(2023, 3, 5, 0, 0, 0, DateTimeKind.Utc),
            NextMaintenanceDate = new DateTime(2023, 9, 5, 0, 0, 0, DateTimeKind.Utc),
            TechnologicalUnitId = 5
        };

        var equipmentInstance7 = new EquipmentInstance
        {
            Id = 7,
            EquipmentTypeId = 3,
            InventoryNumber = "INV-007",
            FactoryNumber = "FAC-1007",
            StationNumber = "ST-007",
            TechnicalNumber = "TECH-007",
            InstallationDate = new DateTime(2019, 12, 10, 0, 0, 0, DateTimeKind.Utc),
            TechnicalObjectId = 6,
            CurrentStatus = "В работе",
            LastMaintenanceDate = new DateTime(2023, 2, 20, 0, 0, 0, DateTimeKind.Utc),
            NextMaintenanceDate = new DateTime(2023, 8, 20, 0, 0, 0, DateTimeKind.Utc),
            TechnologicalUnitId = null
        };

        modelBuilder.Entity<EquipmentInstance>().HasData(
            equipmentInstance1,
            equipmentInstance2,
            equipmentInstance3,
            equipmentInstance4,
            equipmentInstance5,
            equipmentInstance6,
            equipmentInstance7
        );

        // 8. Создаем параметры оборудования
        modelBuilder.Entity<EquipmentParameter>().HasData(
            // Параметры для датчика давления (ID=1)
            new EquipmentParameter
            {
                Id = 1,
                EquipmentInstanceId = 1,
                ParameterName = "Давление",
                MinValue = 0,
                MaxValue = 100,
                NormalValue = 50,
                Unit = "бар",
                AccuracyClass = "0.5",
                MeasurementRange = "0-100 бар"
            },
            new EquipmentParameter
            {
                Id = 2,
                EquipmentInstanceId = 1,
                ParameterName = "Температура",
                MinValue = -20,
                MaxValue = 80,
                NormalValue = 25,
                Unit = "°C",
                AccuracyClass = "1.0",
                MeasurementRange = "-20°C до +80°C"
            },

            // Параметры для контроллера автоматики (ID=2)
            new EquipmentParameter
            {
                Id = 3,
                EquipmentInstanceId = 2,
                ParameterName = "Напряжение питания",
                MinValue = 220,
                MaxValue = 240,
                NormalValue = 230,
                Unit = "В",
                AccuracyClass = "1.0",
                MeasurementRange = "220-240 В"
            },
            new EquipmentParameter
            {
                Id = 4,
                EquipmentInstanceId = 2,
                ParameterName = "Температура окружающей среды",
                MinValue = 0,
                MaxValue = 50,
                NormalValue = 25,
                Unit = "°C",
                AccuracyClass = "1.0",
                MeasurementRange = "0-50°C"
            },

            // Параметры для прибора измерения расхода (ID=3)
            new EquipmentParameter
            {
                Id = 5,
                EquipmentInstanceId = 3,
                ParameterName = "Расход газа",
                MinValue = 0,
                MaxValue = 5000,
                NormalValue = 2500,
                Unit = "м³/ч",
                AccuracyClass = "0.5",
                MeasurementRange = "0-5000 м³/ч"
            },
            new EquipmentParameter
            {
                Id = 6,
                EquipmentInstanceId = 3,
                ParameterName = "Давление",
                MinValue = 0,
                MaxValue = 100,
                NormalValue = 50,
                Unit = "бар",
                AccuracyClass = "0.5",
                MeasurementRange = "0-100 бар"
            }
        );

        // 9. Создаем заявки
        modelBuilder.Entity<Request>().HasData(
            // Новая заявка от заявителя
            new Request
            {
                Id = 1,
                UniqueNumber = "REQ-001",
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                WorkType = WorkType.Repair,
                Priority = Priority.High,
                Status = RequestStatus.New,
                TechnicalSpecs = "Не работает датчик давления на ГРС-1. Необходима замена.",
                Requirements = "Требуется замена датчика давления МП-100 на ГРС-1",
                EquipmentInstanceId = 1,
                TechnicalObjectId = 1,
                WorkshopId = 1,
                TechnologicalUnitId = 1,
                CreatorId = 5, // applicantUser
                AssigneeId = null
            },

            // Заявка в работе
            new Request
            {
                Id = 2,
                UniqueNumber = "REQ-002",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                WorkType = WorkType.Setup,
                Priority = Priority.Medium,
                Status = RequestStatus.InProgress,
                TechnicalSpecs = "Необходима настройка контроллера автоматики AC800 на ГРС-1",
                Requirements = "Настройка параметров контроллера и проверка работы системы",
                EquipmentInstanceId = 2,
                TechnicalObjectId = 1,
                WorkshopId = 1,
                TechnologicalUnitId = 1,
                CreatorId = 5, // applicantUser
                AssigneeId = 3 // executorUser
                
            },

            // Выполненная заявка
            new Request
            {
                Id = 3,
                UniqueNumber = "REQ-003",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                WorkType = WorkType.Installation,
                Priority = Priority.Low,
                Status = RequestStatus.Completed,
                TechnicalSpecs = "Установка нового датчика температуры на газопроводе-10",
                Requirements = "Монтаж и настройка датчика температуры YT-500",
                EquipmentInstanceId = 4,
                TechnicalObjectId = 3,
                WorkshopId = 1,
                TechnologicalUnitId = null,
                CreatorId = 5, // applicantUser
                AssigneeId = 4 // executorUser2
                
            },

            // Отклоненная заявка
            new Request
            {
                Id = 4,
                UniqueNumber = "REQ-004",
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                WorkType = WorkType.Repair,
                Priority = Priority.Critical,
                Status = RequestStatus.Rejected,
                TechnicalSpecs = "Аварийная утечка газа на компрессорной станции КС-5",
                Requirements = "Срочный ремонт узла компрессора, замена уплотнений",
                EquipmentInstanceId = 3,
                TechnicalObjectId = 2,
                WorkshopId = 1,
                TechnologicalUnitId = 2,
                CreatorId = 5, // applicantUser
                AssigneeId = null
            },

            // Еще одна заявка в работе
            new Request
            {
                Id = 5,
                UniqueNumber = "REQ-005",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                WorkType = WorkType.Setup,
                Priority = Priority.High,
                Status = RequestStatus.InProgress,
                TechnicalSpecs = "Настройка системы измерения расхода на резервуаре-1",
                Requirements = "Калибровка прибора Rosemount 8600 и проверка показаний",
                EquipmentInstanceId = 5,
                TechnicalObjectId = 4,
                WorkshopId = 3,
                TechnologicalUnitId = 5,
                CreatorId = 5, // applicantUser
                AssigneeId = 3 // executorUser
                
            }
        );
    }
}
