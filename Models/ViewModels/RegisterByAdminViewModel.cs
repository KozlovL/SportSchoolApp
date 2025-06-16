using System.ComponentModel.DataAnnotations;

// Модель для регистрации пользователя администратором
public class RegisterByAdminViewModel
{
    [Required(ErrorMessage = "Полное имя обязательно")]
    [StringLength(100, ErrorMessage = "Не более 100 символов")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Неверный формат email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Телефон обязателен")]
    [Phone(ErrorMessage = "Неверный формат телефона")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Роль обязательна")]
    public string Role { get; set; } = string.Empty;

    [Display(Name = "Тренер")]
    public string? CoachId { get; set; }
}