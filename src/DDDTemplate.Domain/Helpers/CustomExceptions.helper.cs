using DDDTemplate.Domain.Enums;

namespace DDDTemplate.Domain.Helpers;

public abstract class CustomException(string? message) : Exception(message)
{
  public abstract EResponseCodes ResponseCode { get; }
}

public static class CustomExceptions
{
  public class IdCannotBeNullException() : CustomException("The id field cannot be null.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.BAD_REQUEST;
  }

  public class InvalidDTOException(string resourceName, string errors) : CustomException($"The resource '{resourceName}' contains the following errors: {errors}.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.BAD_REQUEST;
  }

  public class NotFoundException<TId>(string resourceName, TId id) : CustomException($"The resource '{resourceName}' with identifier '{id}' was not found.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.NOT_FOUND;
  }

  public class InvalidFilterOperatorException(string filterOperator) : CustomException($"Unsupported filter operator '{filterOperator}.'")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.BAD_REQUEST;
  }

  public class MethodNotAllowedException(string controllerName) : CustomException($"Method not allowed for the controller: {controllerName}.")
  {
    public override EResponseCodes ResponseCode => EResponseCodes.METHOD_NOT_ALLOWED;
  }
}
