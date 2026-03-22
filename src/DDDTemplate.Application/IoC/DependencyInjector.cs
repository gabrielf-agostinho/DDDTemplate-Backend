using DDDTemplate.Application.Interfaces;
using DDDTemplate.Application.Interfaces.Base;
using DDDTemplate.Application.Services;
using DDDTemplate.Domain.Interfaces.Services;
using DDDTemplate.Domain.Interfaces.Services.Base;
using DDDTemplate.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DDDTemplate.Application.IoC;

public static class DependencyInjector
{
  public static void AddDomainServices(this IServiceCollection services)
  {
    services.Scan(scan => scan
      .FromApplicationDependencies()
      .AddClasses(classes => classes.AssignableTo(typeof(IBaseService<,>)))
      .AsMatchingInterface()
      .WithScopedLifetime()
    );

    services.AddScoped<ICurrentUserService, CurrentUserService>();
  }

  public static void AddApplicationServices(this IServiceCollection services)
  {
    services.Scan(scan => scan
      .FromApplicationDependencies()
      .AddClasses(classes => classes.AssignableTo(typeof(IBaseAppService<,,,,>)))
      .AsMatchingInterface()
      .WithScopedLifetime()
    );

    services.AddScoped<IAuthAppService, AuthAppService>();
  }
}
