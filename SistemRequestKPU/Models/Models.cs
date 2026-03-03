using kursovou_wed.models;


    public class Workshop
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // "Цех 1"
        public string Code { get; set; } = string.Empty; // "C1"
        public int ResponsiblePersonId { get; set; } // FK на User
        public User ResponsiblePerson { get; set; } = null!; // Навигационное свойство

        public ICollection<TechnologicalUnit> TechnologicalUnits { get; set; } = new List<TechnologicalUnit>();
    }

    public class TechnologicalUnit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // "Линия 25"
        public string Code { get; set; } = string.Empty; // "У25"
        public int WorkshopId { get; set; } // FK на Workshop
        public Workshop Workshop { get; set; } = null!;
        public string Description { get; set; } = string.Empty; // Описание процесса

        public ICollection<EquipmentInstance> EquipmentInstances { get; set; } = new List<EquipmentInstance>();
    }

    public class Complex
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // "Комплексная станция"
        public string Type { get; set; } = string.Empty; // "Комплексная станция"
        public string Location { get; set; } = string.Empty; // Местоположение

        public ICollection<TechnicalObject> TechnicalObjects { get; set; } = new List<TechnicalObject>();
    }

    public class TechnicalObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // "ГБ1"
        public int ComplexId { get; set; } // FK на Complex
        public Complex Complex { get; set; } = null!;
        public string ObjectType { get; set; } = string.Empty; // "Газовая ветка"
        public DateTime? InstallationDate { get; set; } // Дата ввода в эксплуатацию

        public ICollection<EquipmentInstance> EquipmentInstances { get; set; } = new List<EquipmentInstance>();
    }

    public class EquipmentType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // "Датчик давления МП-100"
        public string Manufacturer { get; set; } = string.Empty; // Производитель
        public string Model { get; set; } = string.Empty; // Модель
        public string Specifications { get; set; } = string.Empty; // Тех. характеристики

        public ICollection<EquipmentInstance> Instances { get; set; } = new List<EquipmentInstance>();
    }

    public class EquipmentInstance
    {
        public int Id { get; set; }
        public int EquipmentTypeId { get; set; } // FK на EquipmentType
        public EquipmentType EquipmentType { get; set; } = null!;
        public string InventoryNumber { get; set; } = string.Empty; // Инвентарный номер
        public string FactoryNumber { get; set; } = string.Empty; // Заводской номер
        public string StationNumber { get; set; } = string.Empty; // Станционный номер
        public string TechnicalNumber { get; set; } = string.Empty; // Технологический номер
        public DateTime? InstallationDate { get; set; } // Дата установки
        public int TechnicalObjectId { get; set; } // FK на TechnicalObject
        public TechnicalObject TechnicalObject { get; set; } = null!;
        public int? TechnologicalUnitId { get; set; } // FK на TechnologicalUnit (optional)
        public TechnologicalUnit? TechnologicalUnit { get; set; }
        public string CurrentStatus { get; set; } = string.Empty; // "В работе", "На ремонте"
        public DateTime? LastMaintenanceDate { get; set; } // Последнее обслуживание
        public DateTime? NextMaintenanceDate { get; set; } // Следующее обслуживание

        public ICollection<EquipmentParameter> Parameters { get; set; } = new List<EquipmentParameter>();
    }

    public class EquipmentParameter
    {
        public int Id { get; set; }
        public int EquipmentInstanceId { get; set; } // FK на EquipmentInstance
        public EquipmentInstance EquipmentInstance { get; set; } = null!;
        public string ParameterName { get; set; } = string.Empty; // "Давление"
        public double? MinValue { get; set; } // Минимальное значение
        public double? MaxValue { get; set; } // Максимальное
        public double? NormalValue { get; set; } // Нормальное
        public string Unit { get; set; } = string.Empty; // "бар"
        public string AccuracyClass { get; set; } = string.Empty; // Класс точности
        public string MeasurementRange { get; set; } = string.Empty; // Диапазон
    }