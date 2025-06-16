using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportSchoolApp.Models; 

/// Контроллер для управления учетными записями пользователей 
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // Страница входа
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // Обрабатывание данных входа пользователя
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден.");
                return View(model);
            }

            if (string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError(string.Empty, "Некорректные учетные данные.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Неверный email или пароль.");
        }

        return View(model);
    }


    // Страница регистрации
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // Обработка данных регистрации нового пользователя
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

                await _signInManager.SignInAsync(user, isPersistent: false);
                Console.WriteLine("Пользователь успешно вошел.");

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Ошибка при создании пользователя: {error.Description}");
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

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
        var userId = _userManager.GetUserId(User); 
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }
        var user = _userManager.FindByIdAsync(userId).Result; 

        if (user == null)
        {
            return NotFound();
        }

        string? coachName = null;
        if (!string.IsNullOrEmpty(user.CoachId))
        {
            var coach = _userManager.FindByIdAsync(user.CoachId).Result;
            coachName = coach?.FullName;
        }

        var model = new ProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            //Role = user.Role ?? "Не определена",
            CoachName = coachName

        };

        return View(model); 
    }

    // Страница редактирования профиля
    [HttpGet]
    public IActionResult Edit()
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
        {
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
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty
        };

        return View(model);
    }

    // Обработка редактирования
    [HttpPost]
    public async Task<IActionResult> Edit(EditProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    ModelState.AddModelError(string.Empty, "Текущий пароль обязателен для изменения пароля");
                    return View(model);
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(
                    user,
                    model.CurrentPassword,
                    model.NewPassword);

                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
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

