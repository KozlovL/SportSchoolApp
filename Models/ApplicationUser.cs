using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SportSchoolApp.Models
{
  // Модель пользователя, наследуемая от IdentityUser
  public class ApplicationUser : IdentityUser
  {
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public override string? PhoneNumber { get; set; }
    
    public string? Role { get; set; }
    public virtual ICollection<TrainingSession> ConductedSessions { get; set; } = new List<TrainingSession>();
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public string? CoachId { get; set; }
    public virtual ApplicationUser? Coach { get; set; }
    public virtual ICollection<ApplicationUser> Athletes { get; set; } = new List<ApplicationUser>();
    public virtual ICollection<Competition> OrganizedCompetitions { get; set; } = new List<Competition>();
  }
}