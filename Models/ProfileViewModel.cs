using System.ComponentModel.DataAnnotations;

public class ProfileViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;  // Инициализация значением по умолчанию

    [Required]
    public string Email { get; set; } = string.Empty;  // Инициализация значением по умолчанию
}
