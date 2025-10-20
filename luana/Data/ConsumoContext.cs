using Microsoft.EntityFrameworkCore;
using MinimalApiProject.Models;

namespace MinimalApiProject.Data;

public class ConsumoContext : DbContext
{
    public ConsumoContext(DbContextOptions<ConsumoContext> options) : base(options)
    {
    }

    public DbSet<Consumo> Consumos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consumo>()
            .HasIndex(c => new { c.Cpf, c.Mes, c.Ano })
            .IsUnique();
    }
}
