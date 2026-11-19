using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nome = "Vendedor Teste", Email = "vendedor@sa.com", Senha = "123456", Perfil = "VENDEDOR" }
            );
        }
    }
}