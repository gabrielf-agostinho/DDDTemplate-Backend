using Microsoft.Extensions.Options;

namespace DDDTemplate.API.Configurations;

public sealed class ApplicationConfig
{
  public string? Name { get; set; }
}

public static class ApplicationConfigurator
{
  public static ApplicationConfig ReadApplicationConfig(this IConfiguration configuration)
  {
    var section = configuration.GetSection("ApplicationConfig");
    var configurator = new ConfigureFromConfigurationOptions<ApplicationConfig>(section);

    var applicationConfig = new ApplicationConfig();

    configurator.Configure(applicationConfig);

    return applicationConfig;
  }
}
