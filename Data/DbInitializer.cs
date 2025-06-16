using Microsoft.AspNetCore.Identity;
using SportSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace SportSchoolApp.Data
{
    // Инициализация базы данных
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            try
            {
                // Применение миграций
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await context.Database.MigrateAsync();
                }

                await CreateRoles(roleManager);

                await CreateAdminUser(userManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации: {ex.Message}");
            }
        }

        // Создание ролей в системе, если они не существуют
        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { UserRoles.Admin, UserRoles.Coach, UserRoles.Athlete };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    Console.WriteLine($"Создана роль: {role}");
                }
            }
        }

        // Создание главного администратора 
        private static async Task CreateAdminUser(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@sportschool.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Главный Администратор",
                    PhoneNumber = "+79001234567"
                };

                var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                    Console.WriteLine("Администратор создан и добавлен в роль Администратора");
                }
                else
                {
                    Console.WriteLine("Ошибки при создании администратора:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }
                }
            }
        }
    }
}
