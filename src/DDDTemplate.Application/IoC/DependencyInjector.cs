using DDDTemplate.Application.Interfaces.Base;
using DDDTemplate.Domain.Interfaces.Services.Base;
using Microsoft.Extensions.DependencyInjection;

namespace DDDTemplate.Application.IoC;

public static class DependencyInjector
{
  public static void AddDomainServices(this IServiceCollection services)
  {
    services.Scan(scan => scan
      .FromAssembliesOf(typeof(IBaseService<,>))
      .AddClasses(classes => classes.AssignableTo(typeof(IBaseService<,>)))
      .AsMatchingInterface()
      .WithScopedLifetime()
    );
  }

  public static void AddApplicationServices(this IServiceCollection services)
  {
    services.Scan(scan => scan
      .FromAssembliesOf(typeof(IBaseAppService<,,,,>))
      .AddClasses(classes => classes.AssignableTo(typeof(IBaseAppService<,,,,>)))
      .AsMatchingInterface()
      .WithScopedLifetime()
    );
  }
}
