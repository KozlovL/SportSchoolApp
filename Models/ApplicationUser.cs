using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty; // Инициализация по умолчанию
    public string Role { get; set; } = "Athlete"; // Значение по умолчанию для роли

    public ICollection<TrainingSession> ConductedSessions { get; set; } = new List<TrainingSession>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
