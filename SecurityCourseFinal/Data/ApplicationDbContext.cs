using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecurityCourseFinal.Models;

namespace SecurityCourseFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserComment> UserComments { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
    }
}
