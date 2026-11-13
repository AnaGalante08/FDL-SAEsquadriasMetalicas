using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Cliente> Clientes => Set<Cliente>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Cliente>().HasIndex(x => x.Nome).IsUnique();



            mb.Entity<Cliente>().HasData(
     new Cliente
     {
         Id = 1,
         Nome = "Construtora Alfa",
         Email = "contato@alfa.com",
         Telefone = "14999999999"
     },
     new Cliente
     {
         Id = 2,
         Nome = "Maria Souza",
         Email = "maria@email.com",
         Telefone = "14988888888"
     }
 );

        }
    }
}
