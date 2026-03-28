using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Mapster;
using Microsoft.IdentityModel.Tokens;
using DDDTemplate.Application.DTOs;
using DDDTemplate.Application.Interfaces;
using DDDTemplate.Application.Interfaces.Configurations;
using DDDTemplate.Domain.Entities;
using DDDTemplate.Domain.Helpers;
using DDDTemplate.Domain.Interfaces.Services;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DDDTemplate.Application.Services;

public class AuthAppService(IUserService userService, ITokenConfig tokenConfig) : IAuthAppService
{
  protected readonly IUserService UserService = userService;
  protected readonly ITokenConfig TokenConfig = tokenConfig;
  private static readonly JwtSecurityTokenHandler TokenHandler = new();

  private static string RemoveBearer(string authorization) => authorization.Replace("Bearer ", "");
  private DateTime? GetTokenExpirationDate() => TokenConfig.Validation.ValidateLifetime ? DateTime.UtcNow.Add(TokenConfig.Lifetime.AccessTokenExpiration) : null;
  private static Claim GetClaimFromAuthorization(string authorization, string type) => GetClaimsFromAuthorization(authorization).FirstOrDefault(x => x.Type == type) ?? throw new CustomExceptions.InvalidTokenException();

  private static IEnumerable<Claim> GetClaimsFromAuthorization(string authorization)
  {
    var token = RemoveBearer(authorization);

    if (string.IsNullOrEmpty(token) || !TokenHandler.CanReadToken(token))
      throw new CustomExceptions.InvalidTokenException();

    return TokenHandler.ReadJwtToken(token).Claims;
  }

  private static Guid GetUserIdFromAuthorization(string authorization)
  {
    var claim = GetClaimFromAuthorization(authorization, ClaimTypes.NameIdentifier) ?? throw new CustomExceptions.UnauthorizedException("Invalid token.");

    if (!Guid.TryParse(claim.Value, out Guid userId))
      throw new CustomExceptions.UnauthorizedException("Invalid token.");

    return userId;
  }

  private static string GetUserEmailFromAuthorization(string authorization)
  {
    var claim = GetClaimFromAuthorization(authorization, ClaimTypes.Email) ?? throw new CustomExceptions.UnauthorizedException("Invalid token.");

    if (claim.Value is null || string.IsNullOrEmpty(claim.Value))
      throw new CustomExceptions.UnauthorizedException("Invalid token.");

    return claim.Value;
  }

  private static IEnumerable<Claim> GenerateClaims(User user)
  {
    yield return new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
    yield return new Claim(ClaimTypes.Name, user.Name);
    yield return new Claim(ClaimTypes.Email, user.Email);
  }

  private string GenerateAccessToken(User user)
  {
    SymmetricSecurityKey secretKey = new(Encoding.UTF8.GetBytes(TokenConfig.Secret));
    SigningCredentials credentials = new(secretKey, SecurityAlgorithms.HmacSha256);

    JwtSecurityToken token = new(
      issuer: TokenConfig.Validation.Issuer,
      audience: TokenConfig.Validation.Audience,
      signingCredentials: credentials,
      notBefore: DateTime.UtcNow,
      expires: GetTokenExpirationDate(),
      claims: GenerateClaims(user)
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private static string GenerateRefreshToken()
  {
    byte[] randomNumber = new byte[32];
    using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
    randomNumberGenerator.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  private TokenDTO GenerateTokenDTO(User user)
  {
    return new TokenDTO
    {
      AccessToken = GenerateAccessToken(user),
      CreatedAt = DateTime.UtcNow,
      ExpiresAt = GetTokenExpirationDate(),
      RefreshToken = TokenConfig.Validation.ValidateLifetime ? GenerateRefreshToken() : null
    };
  }

  public TokenDTO Auth(AuthDTO authDTO)
  {
    if (!UserService.IsEmailRegistered(authDTO.Email))
      throw new CustomExceptions.EmailNotFoundException(authDTO.Email);

    var user = UserService.GetByEmailAndPassword(authDTO.Email, authDTO.Password) ?? throw new CustomExceptions.WrongPasswordException(authDTO.Email);

    if (!user.IsActive)
      throw new CustomExceptions.UnauthorizedException("User is not active.");

    var token = GenerateTokenDTO(user);

    if (token.RefreshToken is not null)
      UserService.UpdateRefreshToken(user.Id, token.RefreshToken, TokenConfig.Lifetime.RefreshTokenExpiration);

    return token;
  }

  public void Register(UserPostDTO userPostDTO)
  {
    if (UserService.IsEmailRegistered(userPostDTO.Email))
      throw new CustomExceptions.EmailAlreadyRegisteredException(userPostDTO.Email);

    var user = userPostDTO.Adapt<User>();
    UserService.Insert(user);
  }

  public UserGetDTO CurrentUser(string authorization)
  {
    var userId = GetUserIdFromAuthorization(authorization);
    var user = UserService.GetById(userId) ?? throw new CustomExceptions.NotFoundException<Guid>(typeof(User).Name, userId);

    return user.Adapt<UserGetDTO>();
  }

  public void PasswordReset(string authorization, PasswordResetDTO passwordResetDTO)
  {
    var userEmail = GetUserEmailFromAuthorization(authorization);
    var user = UserService.GetByEmailAndPassword(userEmail, passwordResetDTO.OldPassword) ?? throw new CustomExceptions.NotFoundException<string>(typeof(User).Name, userEmail);
    UserService.UpdatePassword(user.Id, passwordResetDTO.NewPassword);
  }

  public TokenDTO Refresh(string authorization, string refreshToken)
  {
    var userId = GetUserIdFromAuthorization(authorization);
    var user = UserService.GetById(userId) ?? throw new CustomExceptions.NotFoundException<Guid>(typeof(User).Name, userId);

    if (refreshToken != user.RefreshToken)
      throw new CustomExceptions.UnauthorizedException("Invalid refresh token.");

    if (DateTime.UtcNow >= user.RefreshTokenExpiration)
      throw new CustomExceptions.UnauthorizedException("Refresh token expired.");

    var token = GenerateTokenDTO(user);

    if (token.RefreshToken is not null)
      UserService.UpdateRefreshToken(user.Id, token.RefreshToken, TokenConfig.Lifetime.RefreshTokenExpiration);

    return token;
  }
}