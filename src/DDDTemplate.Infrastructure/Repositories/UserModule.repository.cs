using DDDTemplate.Domain.Entities;
using DDDTemplate.Domain.Enums;
using DDDTemplate.Domain.Interfaces.Repositories;
using DDDTemplate.Infrastructure.Contexts;
using DDDTemplate.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DDDTemplate.Infrastructure.Repositories;

public class UserModuleRepository(DatabaseContext databaseContext) : BaseRepository<UserModule, int>(databaseContext), IUserModuleRepository
{
  public IEnumerable<EModules> GetModulesByUser(Guid userId)
  {
    return [.. DatabaseContext
      .Set<UserModule>()
      .Where(x => x.UserId == userId)
      .AsNoTracking()
      .Select(x => (EModules)x.ModuleId)];
  }
}