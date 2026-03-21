using DDDTemplate.Domain.Entities;
using DDDTemplate.Domain.Interfaces.Services.Base;

namespace DDDTemplate.Domain.Interfaces.Services;

public interface IUserService : IBaseService<User, Guid>;