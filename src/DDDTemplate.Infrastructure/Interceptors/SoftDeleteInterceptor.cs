using DDDTemplate.Domain.Interfaces.Entities;
using DDDTemplate.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DDDTemplate.Infrastructure.Interceptors;

public class SoftDeleteInterceptor(ICurrentUserService currentUserService) : SaveChangesInterceptor
{
  private readonly ICurrentUserService CurrentUserService = currentUserService;

  public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
  {
    UpdateDeleteFields(eventData.Context);
    return base.SavingChanges(eventData, result);
  }

  private void UpdateDeleteFields(DbContext? context)
  {
    if (context is null) return;

    foreach (var entry in context.ChangeTracker.Entries())
    {
      if (entry.Entity is ISoftDelete<Guid> softDelete)
      {
        if (!softDelete.IsDeleted)
        {
          softDelete.IsDeleted = true;
          softDelete.DeletedAt = DateTime.UtcNow;
          softDelete.DeletedBy = CurrentUserService.UserId.HasValue ? CurrentUserService.UserId : null;
          entry.State = EntityState.Modified;
        }
      }
    }
  }
}