using System.Text.Json.Serialization;
using DDDTemplate.API.Configurations;
using DDDTemplate.API.MIddlewares;
using DDDTemplate.Application.IoC;
using DDDTemplate.Infrastructure.Contexts;
using DDDTemplate.Infrastructure.IoC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Scalar.AspNetCore;

public class Startup(IConfiguration configuration, IWebHostEnvironment environment)
{
  public IConfiguration Configuration { get; } = configuration;
  public IWebHostEnvironment Environment { get; } = environment;

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

  public void ConfigureMapster() => MappingConfig.RegisterMappings();

  public void ConfigureServices(IServiceCollection services)
  {
    services
      .AddControllers(ConfigureControllers)
      .AddJsonOptions(ConfigureJSON);

    services.AddEndpointsApiExplorer();
    services.AddOpenApi();
    services.AddInterceptors();

    if (!Environment.IsEnvironment("Testing"))
    {
      string CONNECTION_STRING = Configuration.GetConnectionString("Database")!;

      services.AddPostgresDatabase<DatabaseContext>(CONNECTION_STRING, true);
      services.AddHealthCheckConfiguration(CONNECTION_STRING);
    }
    else
      services.AddHealthCheckConfiguration();

    services.AddDomainServices();
    services.AddApplicationServices();
    services.AddRepositories();
    services.ConfigureJWT(Configuration.ReadTokenConfig());
  }

  public void Configure(WebApplication app)
  {
    Environment.ApplicationName = Configuration.ReadApplicationConfig().Name!;

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseCors(CorsConfig.DEFAULT_POLICY);
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapHealthCheckConfiguration();
    app.MapControllers();

    if (Environment.IsDevelopment())
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