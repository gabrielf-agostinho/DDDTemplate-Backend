using DDDTemplate.Application.DTOs.Base;
using DDDTemplate.Application.Interfaces.Base;
using DDDTemplate.Domain.Helpers;
using DDDTemplate.Domain.Interfaces.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DDDTemplate.API.Controllers.Base;

public abstract class BaseReadController<TEntity, TId, TGetDTO, TPostDTO, TPutDTO>(IBaseAppService<TEntity, TId, TGetDTO, TPostDTO, TPutDTO> appService, IConfiguration configuration) : BaseController<TEntity, TId, TGetDTO, TPostDTO, TPutDTO>(appService, configuration)
  where TEntity : IEntity<TId>
  where TGetDTO : BaseDTO<TId>
  where TPostDTO : BaseDTO<TId>
  where TPutDTO : BaseDTO<TId>
{
  private IActionResult NotAllowedResponse()
  {
    throw new CustomExceptions.MethodNotAllowedException(ControllerContext.ActionDescriptor.ControllerName);
  }

  public override IActionResult Insert([FromHeader] string Authorization, [FromBody] TPostDTO dto)
  {
    return NotAllowedResponse();
  }

  public override IActionResult Update([FromHeader] string Authorization, [FromBody] TPutDTO dto)
  {
    return NotAllowedResponse();
  }

  public override IActionResult Delete([FromHeader] string Authorization, [FromRoute] TId id)
  {
    return NotAllowedResponse();
  }
}
