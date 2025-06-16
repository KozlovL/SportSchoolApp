using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

// Контроллер для административных функций
[Authorize(Roles = UserRoles.Admin)]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    //Страница регистрации 
    [HttpGet]
    public async Task<IActionResult> RegisterUser()
    {
        var coachUsers = new List<ApplicationUser>();
        foreach (var user in _userManager.Users.ToList())
        {
            if (await _userManager.IsInRoleAsync(user, UserRoles.Coach))
            {
                coachUsers.Add(user);
            }
        }

        ViewBag.Coaches = new SelectList(coachUsers, "Id", "FullName");
        return View();
    }

    //Лбработка данных регистрации
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterUser(RegisterByAdminViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                CoachId = model.Role == UserRoles.Athlete ? model.CoachId : null
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        var coachUsers = new List<ApplicationUser>();
        foreach (var user in await _userManager.Users.ToListAsync())
        {
            if (await _userManager.IsInRoleAsync(user, UserRoles.Coach))
            {
                coachUsers.Add(user);
            }
        }
        ViewBag.Coaches = new SelectList(coachUsers, "Id", "FullName");

        return View(model);
    }

}