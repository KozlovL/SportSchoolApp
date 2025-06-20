// Models/RoleInitializer.cs
using Microsoft.AspNetCore.Identity;
using SportSchoolApp.Models;

namespace SportSchoolApp.Models
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@sportschool.com";
            string password = "Admin123!";

            string[] roles = { Roles.Admin, Roles.Coach, Roles.Athlete };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // создаем администратора
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail, FullName = "Главный Администратор" };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }
    }
}
