using DDDTemplate.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DDDTemplate.API.Helpers;

public class Response(EResponseCodes code, object? data = null, string? errorMessage = null) : IActionResult
{
  public EResponseCodes Code { get; } = code;
  public object? Data { get; } = data;
  public string? ErrorMessage { get; } = errorMessage;

  public bool Success => (int)Code >= (int)EResponseCodes.OK && (int)Code <= (int)EResponseCodes.MULTI_STATUS;

  public Response(EResponseCodes code, Exception exception) : this(code, null, exception.Message) { }

  public async Task ExecuteResultAsync(ActionContext context)
  {
    var responseContent = Success ? Data : new { Error = ErrorMessage };

    if (responseContent is null)
    {
      var statusResult = new StatusCodeResult((int)Code);

      await statusResult.ExecuteResultAsync(context);
    }
    else
    {
      var objectResult = new ObjectResult(responseContent)
      {
        StatusCode = (int)Code
      };

      await objectResult.ExecuteResultAsync(context);
    }
  }
}
