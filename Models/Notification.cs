namespace SportSchoolApp.Models
{
    // Модель недодуманного уведомления
    public class Notification
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}