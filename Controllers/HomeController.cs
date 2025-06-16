using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportSchoolApp.Models;

namespace SportSchoolApp.Controllers;

// Контроллер для главной страницы
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Главная страница
    public IActionResult Index()
    {
        return View();
    }

    // Страница ошибки
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
