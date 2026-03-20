namespace DDDTemplate.Application.DTOs.Base;

public abstract record EnumDTO(int Id, string Description) : BaseDTO<int>(Id);