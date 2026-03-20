using Microsoft.AspNetCore.Cors.Infrastructure;

namespace DDDTemplate.API.Configurations;

public static class CorsConfig
{
  public const string DEFAULT_POLICY = "DEFAULT_POLICY";

  public static void DefaultConfiguration(CorsOptions options)
  {
    options.AddPolicy(DEFAULT_POLICY, builder => builder
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials()
      .WithOrigins([
        "http://localhost:4200"
      ])
    );
  }
}
