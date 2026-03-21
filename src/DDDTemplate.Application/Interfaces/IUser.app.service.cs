using DDDTemplate.Application.DTOs;
using DDDTemplate.Application.Interfaces.Base;
using DDDTemplate.Domain.Entities;

namespace DDDTemplate.Application.Interfaces;

public interface IUserAppService : IBaseAppService<User, Guid, UserGetDTO, UserPostDTO, UserPutDTO>;