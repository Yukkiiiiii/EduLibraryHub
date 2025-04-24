using EduLibraryHub.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace EduLibraryHub.Data.Seeders
{
    public class UserSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        private static async Task SeedUsers(UserManager<User> userManager)
        {
            var admin = new User
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User"
            };
            string adminPassword = "Admin#123";
            await SeedUser(admin, adminPassword, "Admin", userManager);

            var regular = new User()
            {
                UserName = "user@user.com",
                Email = "user@user.com",
                EmailConfirmed = true,
                FirstName = "Regular",
                LastName = "User"
            };
            string regularPassword = "User#123";
            await SeedUser(regular, regularPassword, "User", userManager);
        }

        private static async Task SeedUser(User user, string password, string roleName,
            UserManager<User> userManager)
        {
            var userInfo = await userManager.FindByEmailAsync(user.Email);
            if (userInfo == null)
            {
                var created = await userManager
                    .CreateAsync(user, password);
                if (created.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };
            foreach (var role in roleNames)
            {
                bool roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
