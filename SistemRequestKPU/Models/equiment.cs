namespace kursovou_wed.models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } // Тип оборудования КИПиА
        public string Description { get; set; }
    }

    public class ObjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } // Объект, напр. ГРС
        public string Location { get; set; }
    }
}
