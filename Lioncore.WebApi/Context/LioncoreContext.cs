namespace Lioncore.WebApi.Context;

using Lioncore.WebApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class LioncoreContext : IdentityDbContext<ApplicationUser>
{
    public LioncoreContext(DbContextOptions<LioncoreContext> options)
        : base(options) { }
}
