namespace DDDTemplate.Application.Interfaces.DTOs;

public interface IHasId<TId>
{
  TId? Id { get; init; }
}