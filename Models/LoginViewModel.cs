using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required(ErrorMessage = "Электронная почта обязательна для заполнения.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
