using DDDTemplate.Domain.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DDDTemplate.Infrastructure.IoC;

public static class DependencyInjector
{
  public static void AddRepositories(this IServiceCollection services)
  {
    services.Scan(scan => scan
      .FromApplicationDependencies()
      .AddClasses(classes => classes.AssignableTo(typeof(IBaseRepository<,>)))
      .AsMatchingInterface()
      .WithScopedLifetime()
    );
  }

  public static void AddPostgresDatabase<T>(this IServiceCollection services, string connectionString, bool enableSensitiveDataLogging) where T : DbContext
  {
    services.AddDbContext<T>(database =>
    {
      database.UseSnakeCaseNamingConvention();
      database.EnableSensitiveDataLogging(enableSensitiveDataLogging);
      database.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    });
  }
}