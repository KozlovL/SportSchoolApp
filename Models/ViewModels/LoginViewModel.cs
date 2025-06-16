using System.ComponentModel.DataAnnotations;

// Модель для входа пользователя 
public class LoginViewModel
{
    [Required(ErrorMessage = "Электронная почта обязательна для заполнения.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}