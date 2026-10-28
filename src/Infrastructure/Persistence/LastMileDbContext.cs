using LastMile.Domain.Entregas;
using Microsoft.EntityFrameworkCore;

namespace LastMile.Infrastructure.Persistence;

public sealed class LastMileDbContext : DbContext
{
    public DbSet<Delivery> Deliveries => Set<Delivery>();

    public LastMileDbContext(DbContextOptions<LastMileDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(LastMileDbContext).Assembly);
    }
}