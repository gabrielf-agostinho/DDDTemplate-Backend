using DDDTemplate.Application.DTOs.Base;
using DDDTemplate.Application.Utils;
using DDDTemplate.Domain.Helpers;

namespace DDDTemplate.Application.DTOs;

public record UserGetDTO : CommonGetDTO<Guid>
{
  public string Name { get; init; } = default!;
  public string Email { get; init; } = default!;
}

public record UserPostDTO : CommonPostDTO
{
  public string Name { get; init; } = default!;
  public string Email { get; init; } = default!;
  private string _Password = default!;

  public string Password
  {
    get => _Password;
    init
    {
      if (string.IsNullOrWhiteSpace(value))
        throw new CustomExceptions.InvalidDTOException(nameof(UserPostDTO), "Password is null or empty");

      _Password = HashGenerator.GenerateSHA256(value);
    }
  }
}

public record UserPutDTO : CommonPutDTO<Guid>
{
  public string Name { get; init; } = default!;
  public string Email { get; init; } = default!;
}