namespace SportSchoolApp.Models
{
    // Модель для управления посещаемостью
    public class AttendanceViewModel
    {
        public TrainingSession? Session { get; set; }
        public List<AthleteAttendance>? Athletes { get; set; }
        public List<TrainingSession>? AvailableSessions { get; set; }
    }
}