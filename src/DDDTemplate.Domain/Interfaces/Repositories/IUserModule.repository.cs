using DDDTemplate.Domain.Entities;
using DDDTemplate.Domain.Enums;
using DDDTemplate.Domain.Interfaces.Repositories.Base;

namespace DDDTemplate.Domain.Interfaces.Repositories;

public interface IUserModuleRepository : IBaseRepository<UserModule, int>
{
  IEnumerable<EModules> GetModulesByUser(Guid userId);
}