using System.Text.Json;
using DDDTemplate.API.Helpers;
using DDDTemplate.Domain.Enums;
using DDDTemplate.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DDDTemplate.API.MIddlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
  private readonly RequestDelegate _next = next;

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
  {
    var response = BuildResponse(ex);

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)response.Code;

    var json = JsonSerializer.Serialize(response);

    await context.Response.WriteAsync(json);
  }

  private static Response BuildResponse(Exception ex)
  {
    var code = ResolveStatusCode(ex);

    if (ex is DbUpdateException dbEx && dbEx.InnerException is not null)
      return new Response(code, dbEx.InnerException);

    return new Response(code, ex);
  }

  private static EResponseCodes ResolveStatusCode(Exception ex)
  {
    if (ex is CustomException webEx)
      return webEx.ResponseCode;

    if (ex is DbUpdateException)
      return EResponseCodes.BAD_REQUEST;

    return EResponseCodes.INTERNAL_SERVER_ERROR;
  }
}