using System.ComponentModel.DataAnnotations;

namespace SportSchoolApp.Models
{
    // Модель расписания событий
    public class Schedule
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Тип события")]
        public EventType EventType { get; set; } 

        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Дата и время начала")]
        public DateTime StartDateTime { get; set; }

        [Required]
        [Display(Name = "Дата и время окончания")]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "Место проведения")]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Тренер")]
        public string? CoachId { get; set; }

        [Display(Name = "Тренер")]
        public virtual ApplicationUser? Coach { get; set; }
    }

    // Типы событий в расписании
    public enum EventType
    {
        [Display(Name = "Тренировка")]
        Training,

        [Display(Name = "Тренировочные сборы")]
        Trainingcamp,

        [Display(Name = "Соревнование")]
        Competition,

        [Display(Name = "Турнир")]
        Tournament,

        [Display(Name = "Чемпионат")]
        Championship,

        [Display(Name = "Матч")]
        Match
    }
}