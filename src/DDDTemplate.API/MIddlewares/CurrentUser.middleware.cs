using System.Security.Claims;
using DDDTemplate.Domain.Interfaces.Services;
using DDDTemplate.Domain.Services;

namespace DDDTemplate.API.MIddlewares;

public class CurrentUserMiddleware(RequestDelegate next)
{
  private readonly RequestDelegate _next = next;

  public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
  {
    if (currentUserService is CurrentUserService service)
    {
      var claim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (Guid.TryParse(claim, out var userId))
        service.UserId = userId;
    }

    await _next(context);
  }
}