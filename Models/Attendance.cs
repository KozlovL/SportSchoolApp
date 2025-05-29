public class Attendance
{
    public int Id { get; set; }

    public string AthleteId { get; set; } = string.Empty; // Инициализация по умолчанию
    public ApplicationUser Athlete { get; set; } = new ApplicationUser(); // Инициализация по умолчанию

    public int TrainingSessionId { get; set; }
    public TrainingSession TrainingSession { get; set; } = new TrainingSession(); // Инициализация по умолчанию

    public bool IsPresent { get; set; } = false; // Инициализация по умолчанию
}
