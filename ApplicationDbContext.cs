using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportSchoolApp.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // Конструктор
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Определение DbSet для сущностей
    public DbSet<TrainingSession> TrainingSessions { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
}
