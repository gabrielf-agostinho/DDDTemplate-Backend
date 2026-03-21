using DDDTemplate.Domain.Entities.Base;

namespace DDDTemplate.Domain.Entities;

public class User : CommonEntity<Guid>
{
  public override Guid Id { get; set; } = Guid.NewGuid();
  public required string Name { get; set; }
}