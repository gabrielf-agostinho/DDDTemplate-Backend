namespace DDDTemplate.Application.Interfaces.DTOs;

public interface IAuditableDTO
{
  DateTime? CreatedAt { get; init; }
  DateTime? UpdatedAt { get; init; }
}