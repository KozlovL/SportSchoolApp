using System.ComponentModel.DataAnnotations;

public class EditProfileViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;  // Инициализация значением по умолчанию

    [Required]
    public string Email { get; set; } = string.Empty;  // Инициализация значением по умолчанию
}
