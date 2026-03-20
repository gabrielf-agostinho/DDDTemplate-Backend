namespace DDDTemplate.Domain.Interfaces.Entities;

public interface IUserTrackable<TId>
{
  TId? CreatedBy { get; set; }
  TId? UpdatedBy { get; set; }
}