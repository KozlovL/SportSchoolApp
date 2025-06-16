namespace SportSchoolApp.Models;

// Модель для отображения информации об ошибках
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
