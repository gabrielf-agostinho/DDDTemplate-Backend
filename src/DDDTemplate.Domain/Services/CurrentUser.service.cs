using DDDTemplate.Domain.Interfaces.Services;

namespace DDDTemplate.Domain.Services;

public class CurrentUserService : ICurrentUserService
{
  private static readonly AsyncLocal<Guid?> _userId = new();

  public Guid? UserId
  {
    get => _userId.Value;
    set => _userId.Value = value;
  }
}