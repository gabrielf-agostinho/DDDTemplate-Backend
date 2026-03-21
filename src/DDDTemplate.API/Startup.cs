using System.Text.Json.Serialization;
using DDDTemplate.API.Configurations;
using DDDTemplate.API.MIddlewares;
using DDDTemplate.Application.IoC;
using DDDTemplate.Infrastructure.Contexts;
using DDDTemplate.Infrastructure.IoC;

public class Startup(IConfiguration configuration)
{
  public IConfiguration Configuration { get; } = configuration;

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddControllers().AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      options.JsonSerializerOptions.WriteIndented = true;
    });

    services.AddOpenApi();
    services.AddPostgresDatabase<DatabaseContext>(Configuration.GetConnectionString("Database")!, true);
    services.AddDomainServices();
    services.AddApplicationServices();
    services.AddRepositories();
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
  }
}