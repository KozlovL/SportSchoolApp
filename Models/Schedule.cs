public class Schedule
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty; // Инициализация по умолчанию
    public DateTime WeekStart { get; set; }

    public ICollection<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();
}
