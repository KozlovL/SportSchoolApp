namespace SportSchoolApp.Models
{
    // Модель для представления посещаемости спортсмена
    public class AthleteAttendance
    {
        public string? AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public bool WasPresent { get; set; }
        public string? Notes { get; set; }
    }
}