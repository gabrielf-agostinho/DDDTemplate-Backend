namespace DDDTemplate.Domain.Interfaces.Entities;

public interface ISoftDelete<TId> where TId : struct
{
  bool IsDeleted { get; set; }
  DateTime? DeletedAt { get; set; }
  TId? DeletedBy { get; set; }
}