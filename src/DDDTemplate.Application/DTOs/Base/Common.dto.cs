namespace DDDTemplate.Application.DTOs.Base;

public abstract record CommonDTO<TId>(
  TId Id, 
  bool IsActive, 
  DateTime? CreatedAt, 
  DateTime? UpdatedAt,
  bool IsDeleted,
  DateTime? DeletedAt,
  TId? CreatedBy,
  TId? UpdatedBy
) : BaseDTO<TId>(Id);