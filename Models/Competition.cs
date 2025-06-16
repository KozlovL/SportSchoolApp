namespace SportSchoolApp.Models
{
    // Модель соревнования
    public class Competition
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }

    }
}