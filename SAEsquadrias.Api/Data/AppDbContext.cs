using Microsoft.EntityFrameworkCore;
using SAEsquadrias.Api.Models;

namespace SAEsquadrias.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Vendedor> Vendedores => Set<Vendedor>();
        public DbSet<Administrador> Administradores => Set<Administrador>();
        public DbSet<Obra> Obras { get; set; }
        public DbSet<ItemObra> ItensObras { get; set; }
        public DbSet<Pendencia> Pendencias { get; set; }
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<EtapaObra> EtapasObra { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<ListaObra> ListaObras { get; set; }
        public DbSet<RegistroCampo> RegistrosCampo => Set<RegistroCampo>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Cliente>()
                .HasIndex(x => x.Nome)
                .IsUnique();

            mb.Entity<Pendencia>()
                .HasOne(p => p.Obra)
                .WithMany()
                .HasForeignKey(p => p.ProjetoId)
                .OnDelete(DeleteBehavior.NoAction);

            mb.Entity<Pendencia>()
                .HasOne(p => p.Vendedor)
                .WithMany()
                .HasForeignKey(p => p.VendedorId)
                .OnDelete(DeleteBehavior.NoAction);

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
