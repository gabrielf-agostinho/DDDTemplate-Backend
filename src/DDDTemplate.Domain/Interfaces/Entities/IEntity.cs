namespace DDDTemplate.Domain.Interfaces.Entities;

public interface IEntity<TId>
{
  TId Id { get; set; }
}