using DDDTemplate.Domain.Enums;

namespace DDDTemplate.Domain.Helpers;

public abstract class CustomException(string? message) : Exception(message)
{
  public abstract EResponseCodes ResponseCode { get; }
}

public static class CustomExceptions
{
  public class IdCannotBeNullException() : CustomException("O campo id não pode ser nulo.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.BAD_REQUEST;
  }

  public class InvalidDTOException(string resourceName, string errors) : CustomException($"O recurso '{resourceName}' contém os seguintes erros: {errors}.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.BAD_REQUEST;
  }

  public class NotFoundException<TId>(string resourceName, TId id) : CustomException($"O recurso '{resourceName}' com o identificador '{id}' não foi encontrado.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.NOT_FOUND;
  }

  public class InvalidFilterOperatorException(string filterOperator) : CustomException($"Operador de filtro não suportado '{filterOperator}'.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.BAD_REQUEST;
  }

  public class MethodNotAllowedException(string controllerName) : CustomException($"Método não permitido para o controller: {controllerName}.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.METHOD_NOT_ALLOWED;
  }

  public class EmailNotFoundException(string email) : CustomException($"Não existe usuário cadastrado com o e-mail {email}.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.NOT_FOUND;
  }

  public class EmailAlreadyRegisteredException(string email) : CustomException($"O e-mail {email} já está cadastrado.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.BAD_REQUEST;
  }

  public class WrongPasswordException(string email) : CustomException($"Senha incorreta para o e-mail: {email}.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.BAD_REQUEST;
  }

  public class InvalidTokenException() : CustomException("Token inválido.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.BAD_REQUEST;
  }

  public class UnauthorizedException(string message) : CustomException(message)
  {
    public override EResponseCodes ResponseCode => EResponseCodes.UNAUTHORIZED;
  }
}
