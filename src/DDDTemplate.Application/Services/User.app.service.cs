using DDDTemplate.Application.DTOs;
using DDDTemplate.Application.Interfaces;
using DDDTemplate.Application.Services.Base;
using DDDTemplate.Domain.Entities;
using DDDTemplate.Domain.Interfaces.Services;

namespace DDDTemplate.Application.Services;

public class UserAppService(IUserService userService) : BaseAppService<User, Guid, UserGetDTO, UserPostDTO, UserPutDTO>(userService), IUserAppService;