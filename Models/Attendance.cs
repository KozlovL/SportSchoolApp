using System.ComponentModel.DataAnnotations;

namespace SportSchoolApp.Models
{
    // Модель посещения 
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        public string AthleteId { get; set; } = string.Empty;
        public ApplicationUser? Athlete { get; set; }

        [Required]
        public int TrainingSessionId { get; set; }
        public TrainingSession? TrainingSession { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public bool WasPresent { get; set; } = true;
        public string Notes { get; set; } = string.Empty;
    }
}