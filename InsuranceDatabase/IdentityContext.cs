using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace LibraryMVC.Models
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext(DbContextOptions <IdentityContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
}
}
