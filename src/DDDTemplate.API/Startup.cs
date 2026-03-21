using System.Text.Json.Serialization;
using DDDTemplate.API.Configurations;
using DDDTemplate.API.MIddlewares;
using DDDTemplate.Application.IoC;
using DDDTemplate.Infrastructure.Contexts;
using DDDTemplate.Infrastructure.IoC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Scalar.AspNetCore;

public class Startup(IConfiguration configuration)
{
  public IConfiguration Configuration { get; } = configuration;

  private static void ConfigureControllers(MvcOptions options)
  {
    options.Conventions.Add(
      new RouteTokenTransformerConvention(
        new SlugfyParameter()
      )
    );
  }

  private static void ConfigureJSON(JsonOptions options)
  {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
  }

  public void ConfigureServices(IServiceCollection services)
  {
    services
      .AddControllers(ConfigureControllers)
      .AddJsonOptions(ConfigureJSON);

    services.AddEndpointsApiExplorer();
    services.AddOpenApi();
    services.AddPostgresDatabase<DatabaseContext>(Configuration.GetConnectionString("Database")!, true);
    services.AddDomainServices();
    services.AddApplicationServices();
    services.AddRepositories();
    services.ConfigureJWT(Configuration.ReadTokenConfig());
  }

  public void Configure(WebApplication app, IWebHostEnvironment environment)
  {
    environment.ApplicationName = Configuration.ReadApplicationConfig().Name!;

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseCors(CorsConfig.DEFAULT_POLICY);
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    if (environment.IsDevelopment())
    {
      app.MapOpenApi();

      app.MapScalarApiReference(options =>
      {
        options.ShowDeveloperTools = DeveloperToolsVisibility.Never;
        options.DarkMode = true;
        options.Title = "DDDTemplate API";
      });
    }
  }
}