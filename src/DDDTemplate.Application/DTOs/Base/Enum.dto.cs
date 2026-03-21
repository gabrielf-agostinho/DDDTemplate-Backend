namespace DDDTemplate.Application.DTOs.Base;

public abstract record EnumDTO : BaseDTO<int>
{
  public required string Description { get; init; }
}