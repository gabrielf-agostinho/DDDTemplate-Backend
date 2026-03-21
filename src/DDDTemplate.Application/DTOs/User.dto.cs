using DDDTemplate.Application.DTOs.Base;

namespace DDDTemplate.Application.DTOs;

public record UserGetDTO(CommonDTO<Guid> Original, string Name) : CommonDTO<Guid>(Original);
public record UserPostDTO(CommonDTO<Guid> Original, string Name) : CommonDTO<Guid>(Original);
public record UserPutDTO(CommonDTO<Guid> Original, string Name) : CommonDTO<Guid>(Original);