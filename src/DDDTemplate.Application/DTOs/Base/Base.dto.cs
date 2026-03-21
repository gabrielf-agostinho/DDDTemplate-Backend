using DDDTemplate.Application.Interfaces.DTOs;

namespace DDDTemplate.Application.DTOs.Base;

public abstract record BaseDTO<TId> : IHasId<TId>
{
  public TId? Id { get; init; }
}