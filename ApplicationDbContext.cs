using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportSchoolApp.Models;

// Контекст БД, наследуемый от IdentityDbContext
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // Конструктор контекста
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Наборы сущностей в БД
    public DbSet<TrainingSession> TrainingSessions { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Competition> Competitions { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    /// Конфигурация модели БД
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Athletes)
            .WithOne(u => u.Coach)
            .HasForeignKey(u => u.CoachId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Schedule>()
           .HasOne(s => s.Coach)
           .WithMany()
           .HasForeignKey(s => s.CoachId)
           .OnDelete(DeleteBehavior.Restrict);
    }

}
