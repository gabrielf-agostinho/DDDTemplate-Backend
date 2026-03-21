using DDDTemplate.API.Helpers;
using DDDTemplate.Application.DTOs.Base;
using DDDTemplate.Application.Interfaces.Base;
using DDDTemplate.Domain.Enums;
using DDDTemplate.Domain.Helpers;
using DDDTemplate.Domain.Interfaces.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDDTemplate.API.Controllers.Base;

[ApiController]
[Authorize]
[Produces("application/json")]
[Route("api/v1/[controller]")]
public abstract class BaseController<TEntity, TId, TGetDTO, TPostDTO, TPutDTO>(IBaseAppService<TEntity, TId, TGetDTO, TPostDTO, TPutDTO> appService, IConfiguration configuration) : Controller
  where TEntity : IEntity<TId>
  where TGetDTO : BaseDTO<TId>
  where TPutDTO : BaseDTO<TId>
  where TPostDTO : class
{
  protected readonly IBaseAppService<TEntity, TId, TGetDTO, TPostDTO, TPutDTO> AppService = appService;
  protected readonly IConfiguration Configuration = configuration;

  [HttpGet]
  [Route("")]
  public virtual IActionResult GetAll()
  {
    Filter<TEntity, TId>? filter = FilterBuilder.Build<TEntity, TId>(Request.Query);

    if (filter is not null)
      return new Response(EResponseCodes.OK, AppService.GetPaged(filter.Skip, filter.Take, filter.Params, filter.OrderByField, filter.OrderByDirection));
    else
      return new Response(EResponseCodes.OK, AppService.GetAll());
  }

  [HttpGet]
  [Route("{id:int}")]
  public virtual IActionResult GetById([FromRoute] TId id)
  {
    TGetDTO dto = AppService.GetById(id) ?? throw new CustomExceptions.NotFoundException<TId>(typeof(TEntity).Name, id);
    return new Response(EResponseCodes.OK, dto);
  }

  [HttpPost]
  [Route("")]
  public virtual IActionResult Insert([FromBody] TPostDTO dto)
  {
    DTOValidator.CheckForErrors<TPostDTO>(ViewData);

    TId id = AppService.Insert(dto);
    return new Response(EResponseCodes.CREATED, new { id });
  }

  [HttpPut]
  [Route("")]
  public virtual IActionResult Update([FromBody] TPutDTO dto)
  {
    DTOValidator.CheckForErrors<TPutDTO>(ViewData);

    AppService.Update(dto);
    return new Response(EResponseCodes.OK);
  }

  [HttpDelete]
  [Route("{id:int}")]
  public virtual IActionResult Delete([FromRoute] TId id)
  {
    AppService.Delete(id);
    return new Response(EResponseCodes.OK);
  }
}
