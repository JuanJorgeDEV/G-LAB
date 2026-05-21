using Microsoft.EntityFrameworkCore;

namespace ProjetoOS.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Equipamento> Equipamentos => Set<Equipamento>();
    public DbSet<OrdemServico> OrdensServico => Set<OrdemServico>();
    public DbSet<RegistroTempo> RegistrosTempo => Set<RegistroTempo>();
    public DbSet<HistoricoOS> HistoricosOS => Set<HistoricoOS>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipamento>()
            .HasIndex(e => e.NI)
            .IsUnique()
            .HasFilter("\"NI\" IS NOT NULL AND \"NI\" <> ''");

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
