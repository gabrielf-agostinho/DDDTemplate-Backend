namespace DDDTemplate.Domain.Interfaces.Services;

public interface ICurrentUserService
{
  Guid? UserId { get; }
}