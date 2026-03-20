namespace DDDTemplate.Domain.Interfaces.Entities;

public interface IUserEntity<TId>
{
  TId UserId { get; set; }
}