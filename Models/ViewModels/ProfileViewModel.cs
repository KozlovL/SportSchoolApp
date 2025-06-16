using System.ComponentModel.DataAnnotations;

// Модель для отображения профиля пользователя
public class ProfileViewModel
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "Полное имя")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Телефон")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Роль")]
    public string Role { get; set; } = string.Empty;

    [Display(Name = "Тренер")]
    public string? CoachName { get; set; }
    public Guid? CoachId { get; set; }
}