namespace DDDTemplate.Application.Interfaces.DTOs;

public interface ISoftDeleteDTO
{
  bool IsDeleted { get; init; }
  DateTime? DeletedAt { get; init; }
}