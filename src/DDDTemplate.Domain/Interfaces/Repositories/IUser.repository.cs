using DDDTemplate.Domain.Entities;
using DDDTemplate.Domain.Interfaces.Repositories.Base;

namespace DDDTemplate.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User, Guid>;