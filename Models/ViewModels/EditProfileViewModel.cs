using System.ComponentModel.DataAnnotations;

// Модель для редактирования профиля пользователя
public class EditProfileViewModel
{
    [Required(ErrorMessage = "Полное имя обязательно")]
    [Display(Name = "ФИО")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Неверный формат email")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Неверный формат телефона")]
    [Display(Name = "Номер телефона")]
    public string PhoneNumber { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Новый пароль")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов")]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Подтвердите пароль")]
    [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
    public string? ConfirmPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Текущий пароль (для подтверждения изменений)")]
    public string? CurrentPassword { get; set; }
}