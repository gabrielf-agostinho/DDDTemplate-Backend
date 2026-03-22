namespace DDDTemplate.Domain.Interfaces.Entities;

public interface IUserTrackable<TId> where TId : struct
{
  TId? CreatedBy { get; set; }
  TId? UpdatedBy { get; set; }
}