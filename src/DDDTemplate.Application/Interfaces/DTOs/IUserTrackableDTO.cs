namespace DDDTemplate.Application.Interfaces.DTOs;

public interface IUserTrackableDTO<TId>
{
  TId? CreatedBy { get; init; }
  TId? UpdatedBy { get; init; }
}