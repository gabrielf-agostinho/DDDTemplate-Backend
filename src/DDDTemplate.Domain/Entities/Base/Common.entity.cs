using DDDTemplate.Domain.Interfaces.Entities;

namespace DDDTemplate.Domain.Entities.Base;

public abstract class CommonEntity<TId> : IEntity<TId>, IActivatable, IAuditable, ISoftDelete<TId>, IUserTrackable<TId> where TId : struct
{
  public abstract TId Id { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime? CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public bool IsDeleted { get; set; }
  public DateTime? DeletedAt { get; set; }
  public TId? DeletedBy { get; set; }
  public TId? CreatedBy { get; set; }
  public TId? UpdatedBy { get; set; }
}