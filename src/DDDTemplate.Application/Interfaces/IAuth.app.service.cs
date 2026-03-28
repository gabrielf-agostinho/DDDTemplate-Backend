using DDDTemplate.Application.DTOs;

namespace DDDTemplate.Application.Interfaces;

public interface IAuthAppService
{
  TokenDTO Auth(AuthDTO authDTO);
  void Register(UserPostDTO userPostDTO);
  TokenDTO Refresh(string authorization, string refreshToken);
  UserGetDTO CurrentUser(string authorization);
  void PasswordReset(string authorization, PasswordResetDTO passwordResetDTO);
}