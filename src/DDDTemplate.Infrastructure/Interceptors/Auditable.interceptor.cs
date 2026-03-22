using DDDTemplate.Domain.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DDDTemplate.Infrastructure.Interceptors;

public class AuditableInterceptor : SaveChangesInterceptor
{
  public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
  {
    UpdateAuditFields(eventData.Context);
    return base.SavingChanges(eventData, result);
  }

  private static void UpdateAuditFields(DbContext? context)
  {
    if (context is null) return;

    var now = DateTime.UtcNow;

    foreach (var entry in context.ChangeTracker.Entries())
    {
      if (entry.Entity is IAuditable auditable)
      {
        if (entry.State == EntityState.Added)
          auditable.CreatedAt = now;

        if (entry.State == EntityState.Modified)
          auditable.UpdatedAt = now;
      }
    }
  }
}