using DDDTemplate.API.Controllers.Base;
using DDDTemplate.Application.DTOs;
using DDDTemplate.Application.Interfaces;
using DDDTemplate.Domain.Entities;

namespace DDDTemplate.API.Controllers;

public class UsersController(IUserAppService userAppService, IConfiguration configuration) : BaseController<User, Guid, UserGetDTO, UserPostDTO, UserPutDTO>(userAppService, configuration);