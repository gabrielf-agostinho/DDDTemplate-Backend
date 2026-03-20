using DDDTemplate.Domain.Interfaces.Entities;

namespace DDDTemplate.Domain.Entities.Base;

public abstract class EnumEntity : IEntity<int>, IHasDescription
{
  public int Id { get; set; }
  public required string Description { get; set; }
}
