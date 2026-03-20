using Microsoft.EntityFrameworkCore;

namespace DDDTemplate.Infrastructure.Contexts;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
  }
}