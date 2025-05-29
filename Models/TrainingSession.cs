public class TrainingSession
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty; // Инициализация по умолчанию
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = string.Empty; // Инициализация по умолчанию

    // Связь с тренером
    public string CoachId { get; set; } = string.Empty; // Инициализация по умолчанию
    public ApplicationUser Coach { get; set; } = new ApplicationUser(); // Инициализация по умолчанию

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
