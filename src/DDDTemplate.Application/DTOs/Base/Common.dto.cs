using DDDTemplate.Application.Interfaces.DTOs;

namespace DDDTemplate.Application.DTOs.Base;

public abstract record CommonGetDTO<TId> :
  BaseDTO<TId>,
  IActivatableDTO,
  IAuditableDTO,
  IUserTrackableDTO<TId>,
  ISoftDeleteDTO
{
  public bool IsActive { get; init; }
  public DateTime? CreatedAt { get; init; }
  public DateTime? UpdatedAt { get; init; }
  public bool IsDeleted { get; init; }
  public DateTime? DeletedAt { get; init; }
  public TId? CreatedBy { get; init; }
  public TId? UpdatedBy { get; init; }
}

public abstract record CommonPostDTO : IActivatableDTO
{
  public bool IsActive { get; init; } = true;
}

public abstract record CommonPutDTO<TId> :
    BaseDTO<TId>,
    IActivatableDTO
{
  public bool IsActive { get; init; }
}