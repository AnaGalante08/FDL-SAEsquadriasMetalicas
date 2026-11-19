using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
        }
    }
}
