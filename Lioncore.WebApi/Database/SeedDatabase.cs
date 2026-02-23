using Lioncore.WebApi.Context;
using Lioncore.WebApi.Entities;
using Lioncore.WebApi.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lioncore.WebApi.Database;

public static class SeedDatabase
{
    public static void Initialize(IServiceProvider services)
    {
        var context = services.GetRequiredService<LioncoreContext>();

        // context.Database.EnsureDeleted();

        context.Database.Migrate();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var configuration = services.GetRequiredService<IConfiguration>();
        var password = configuration["Root:Password"];

        if (string.IsNullOrEmpty(password))
        {
            throw new NullReferenceException("Root Password Cant be Empty");
        }

        if (context.Users.Any() == false)
        {
            var root = new ApplicationUser
            {
                UserName = configuration["Root:Email"],
                Email = configuration["Root:Email"],
                FullName = "Root Root",
                EmailConfirmed = true,
            };

            userManager.CreateAsync(root, password).Wait();
        }

        if (context.Roles.Any() == false)
        {
            roleManager.CreateAsync(new IdentityRole(Roles.Root)).Wait();
            roleManager.CreateAsync(new IdentityRole(Roles.Admin)).Wait();
        }
    }
}
