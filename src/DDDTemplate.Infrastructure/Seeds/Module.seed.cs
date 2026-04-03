using DDDTemplate.Domain.Entities;
using DDDTemplate.Domain.Enums;

namespace DDDTemplate.Infrastructure.Seeds;

public static class ModuleSeed
{
  public static Module[] Seed()
  {
    return [
      Administrative(),
      Users()
    ];
  }

  private static Module Administrative()
  {
    return new Module
    {
      Id = (int)EModules.ADMINISTRATIVE,
      Label = "Administrativo",
      Icon = "pi pi-building",
    };
  }

  private static Module Users()
  {
    return new Module
    {
      ParentId = (int)EModules.ADMINISTRATIVE,
      Id = (int)EModules.USERS,
      Label = "Usuários",
      Icon = "pi pi-users"
    };
  }
}