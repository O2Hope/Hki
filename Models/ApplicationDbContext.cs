using hki.web.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hki.web.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
            
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<ApplicationUser> User { get; set; }

        public DbSet<Orden> Ordenes { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Piezas> Piezas { get; set; }
        
    }
}