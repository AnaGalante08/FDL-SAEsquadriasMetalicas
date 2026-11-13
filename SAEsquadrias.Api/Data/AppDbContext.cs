using Microsoft.EntityFrameworkCore;
using SAEsquadrias.API.Models;

namespace SAEsquadrias.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<Obra> Obras { get; set; }

        public DbSet<EtapaObra> EtapasObra { get; set; }
        public DbSet<Produto> Produtos { get; set; }

    }
}
