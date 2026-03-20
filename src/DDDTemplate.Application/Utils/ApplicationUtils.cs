using DDDTemplate.Application.DTOs.Base;
using DDDTemplate.Domain.Helpers;

namespace DDDTemplate.Application.Utils;

public static class ApplicationUtils
{
  public static void CheckIdField<TPutDTO, TId>(TPutDTO dto) where TPutDTO : BaseDTO<TId>
  {
    if (dto.Id is null)
      throw new CustomExceptions.IdCannotBeNullException();
  }
}
