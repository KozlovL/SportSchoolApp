using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportSchoolApp.Models; // Используем модель ApplicationUser для Identity
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неудачная попытка входа.");
            }
        }

        return View(model);
    }


        // Страница регистрации
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // Обработка регистрации
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
{
    if (ModelState.IsValid)
    {
        if (model.Password != model.ConfirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Пароли не совпадают.");
            Console.WriteLine("Пароли не совпадают");
            return View(model);
        }

        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            Console.WriteLine("Пользователь успешно создан.");

            // Логируем успешный вход
            await _signInManager.SignInAsync(user, isPersistent: false);
            Console.WriteLine("Пользователь успешно вошел.");

            // Если нет returnUrl, то перенаправляем на домашнюю страницу
            return RedirectToAction("Index", "Home");
        }

        // Если есть ошибки при создании пользователя, выводим их в консоль
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Ошибка при создании пользователя: {error.Description}");
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    // Если модель не валидна, выводим сообщение
    Console.WriteLine("Модель не валидна.");
    return View(model);
}



    // Страница выхода
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // Страница профиля
    [HttpGet]
    public IActionResult Profile()
    {
        var userId = _userManager.GetUserId(User); // Получаем ID текущего пользователя
        if (userId == null)
            {
                // Обработка ошибки, если userId равен null
                return RedirectToAction("Login", "Account");
            }
        var user = _userManager.FindByIdAsync(userId).Result; // Находим пользователя по ID

        if (user == null)
        {
            return NotFound();
        }

        var model = new ProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email ?? string.Empty
        };

        return View(model); // Отправляем модель на представление
    }

    // Страница редактирования профиля
[HttpGet]
public IActionResult Edit()
{
    var userId = _userManager.GetUserId(User); // Получаем ID текущего пользователя
    if (userId == null)
        {
            // Обработка ошибки, если userId равен null
            return RedirectToAction("Login", "Account");
        }
    var user = _userManager.FindByIdAsync(userId).Result;

    if (user == null)
    {
        return NotFound();
    }

    var model = new EditProfileViewModel
    {
        FullName = user.FullName,
        Email = user.Email ?? string.Empty
    };

    return View(model);
}

// Обработка редактирования
[HttpPost]
public async Task<IActionResult> Edit(EditProfileViewModel model)
{
    if (ModelState.IsValid)
    {
        var userId = _userManager.GetUserId(User); // Получаем ID текущего пользователя
        if (userId == null)
            {
                // Обработка ошибки, если userId равен null
                return RedirectToAction("Login", "Account");
            }
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        user.FullName = model.FullName;
        user.Email = model.Email;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return RedirectToAction("Profile");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    return View(model);
}


}

