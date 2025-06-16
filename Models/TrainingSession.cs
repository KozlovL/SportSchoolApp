using System.ComponentModel.DataAnnotations;

namespace SportSchoolApp.Models
{
    // Модель тренировочной сессии
    public class TrainingSession
    {
        public int Id { get; set; }

        [Required]
        public string CoachId { get; set; } = string.Empty;
        public ApplicationUser? Coach { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string? Location { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public int? ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
    }
}