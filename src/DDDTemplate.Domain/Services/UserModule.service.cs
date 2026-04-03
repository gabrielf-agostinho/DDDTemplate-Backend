using DDDTemplate.Domain.Entities;
using DDDTemplate.Domain.Enums;
using DDDTemplate.Domain.Interfaces.Repositories;
using DDDTemplate.Domain.Interfaces.Services;
using DDDTemplate.Domain.Services.Base;

namespace DDDTemplate.Domain.Services;

public class UserModuleService(
  ICurrentUserService currentUserService,
  IUserModuleRepository userModuleRepository,
  IModuleRepository moduleRepository
) : BaseService<UserModule, int>(userModuleRepository), IUserModuleService
{
  protected readonly ICurrentUserService CurrentUserService = currentUserService;
  protected readonly IUserModuleRepository UserModuleRepository = userModuleRepository;
  protected readonly IModuleRepository ModuleRepository = moduleRepository;

  public IEnumerable<Module> GetByCurrentUser()
  {
    var userModulesIds = UserModuleRepository.GetModulesByUser((Guid)CurrentUserService.UserId!);
    var allModules = ModuleRepository.GetAll();
    var allowedModules = GetAllowedModulesWithParents(allModules, userModulesIds);

    return BuildTree(null, allowedModules);
  }

  private static List<Module> GetAllowedModulesWithParents(IEnumerable<Module> allModules, IEnumerable<EModules> moduleIds)
  {
    var allowed = new HashSet<EModules>(moduleIds);

    bool added;

    do
    {
      added = false;

      foreach (var module in allModules)
      {
        if (module.ParentId.HasValue && allowed.Contains((EModules)module.Id) && !allowed.Contains((EModules)module.ParentId.Value))
        {
          allowed.Add((EModules)module.ParentId.Value);
          added = true;
        }
      }

    } while (added);

    return allModules.Where(m => allowed.Contains((EModules)m.Id)).ToList();
  }

  private static List<Module> BuildTree(int? parentId, List<Module> modules)
  {
    return modules
      .Where(m => m.ParentId == parentId)
      .Select(m => new Module
      {
        Id = m.Id,
        Label = m.Label,
        Icon = m.Icon,
        ParentId = m.ParentId,
        Items = BuildTree(m.Id, modules)
      })
      .ToList();
  }
}