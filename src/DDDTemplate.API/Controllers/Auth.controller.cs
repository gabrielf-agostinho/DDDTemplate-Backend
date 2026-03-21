using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DDDTemplate.API.Helpers;
using DDDTemplate.Domain.Enums;
using DDDTemplate.Application.DTOs;
using DDDTemplate.Application.Interfaces;

namespace DDDTemplate.API.Controllers;

[ApiController]
[Authorize]
[Produces("application/json")]
[Route("api/v1/[controller]")]
public class AuthController(IAuthAppService authAppService) : Controller
{
  protected readonly IAuthAppService AuthAppService = authAppService;

  [HttpPost]
  [Route("")]
  [AllowAnonymous]
  public virtual IActionResult Auth([FromBody] AuthDTO authDTO)
  {
    DTOValidator.CheckForErrors<AuthDTO>(ViewData);
    return new Response(EResponseCodes.OK, AuthAppService.Auth(authDTO));
  }

  [HttpPatch]
  [Route("refresh")]
  [AllowAnonymous]
  public virtual IActionResult Refresh([FromHeader] string authorization, [FromHeader] string refreshToken)
  {
    return new Response(EResponseCodes.OK, AuthAppService.Refresh(authorization, refreshToken));
  }

  [HttpGet]
  [Route("current-user")]
  public virtual IActionResult CurrentUser([FromHeader] string authorization)
  {
    return new Response(EResponseCodes.OK, AuthAppService.CurrentUser(authorization));
  }

  [HttpPatch]
  [Route("password-reset")]
  public virtual IActionResult PasswordReset([FromHeader] string authorization, [FromBody] PasswordResetDTO passwordResetDTO)
  {
    DTOValidator.CheckForErrors<PasswordResetDTO>(ViewData);
    AuthAppService.PasswordReset(authorization, passwordResetDTO);
    return new Response(EResponseCodes.OK);
  }
}