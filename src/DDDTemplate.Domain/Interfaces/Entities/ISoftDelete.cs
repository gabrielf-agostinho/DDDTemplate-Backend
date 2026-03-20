namespace DDDTemplate.Domain.Interfaces.Entities;

public interface ISoftDelete
{
  bool IsDeleted { get; set; }
  DateTime? DeletedAt { get; set; }
}