using Microsoft.AspNetCore.Identity;

namespace OOPLab1
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            string username = "Admin";
            string password = "Qwerty_1";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await userManager.FindByNameAsync(username) == null)
            {
                IdentityUser admin = new IdentityUser { UserName = username};
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }

    }
}
