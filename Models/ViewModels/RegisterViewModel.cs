using System.ComponentModel.DataAnnotations;

// Модель для регистрации пользователя администратором
public class RegisterViewModel
{
    [Required(ErrorMessage = "Электронная почта обязательна для заполнения.")]
    [EmailAddress(ErrorMessage = "Неверный формат электронной почты.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Полное имя обязательно для заполнения.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Подтверждение пароля обязательно.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
