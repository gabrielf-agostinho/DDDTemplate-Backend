using DDDTemplate.Domain.Interfaces.Repositories.Base;
using DDDTemplate.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

  public static void AddInterceptors(this IServiceCollection services)
  {
    services.AddScoped<AuditableInterceptor>();
    services.AddScoped<UserTrackableInterceptor>();
  }

  public static void AddPostgresDatabase<T>(this IServiceCollection services, string connectionString, bool enableSensitiveDataLogging) where T : DbContext
  {
    var serviceProvider = services.BuildServiceProvider();

    services.AddDbContext<T>(database =>
    {
      database.UseSnakeCaseNamingConvention();
      database.EnableSensitiveDataLogging(enableSensitiveDataLogging);
      database.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
      database.AddInterceptors(GetInterceptors(serviceProvider));
    });
  }

  private static IEnumerable<IInterceptor> GetInterceptors(ServiceProvider serviceProvider)
  {
    yield return serviceProvider.GetRequiredService<AuditableInterceptor>();
    yield return serviceProvider.GetRequiredService<UserTrackableInterceptor>();
  }
}