using DDDTemplate.Domain.Interfaces.Entities;
using DDDTemplate.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DDDTemplate.Infrastructure.Interceptors;

public class UserTrackableInterceptor(ICurrentUserService currentUserService) : SaveChangesInterceptor
{
  private readonly ICurrentUserService CurrentUserService = currentUserService;

  public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
  {
    UpdateUserTrackableFields(eventData.Context);
    return base.SavingChanges(eventData, result);
  }

  private void UpdateUserTrackableFields(DbContext? context)
  {
    if (context is null) return;

    foreach (var entry in context.ChangeTracker.Entries())
    {
      if (entry.Entity is IUserTrackable<Guid> userTrackable)
      {
        if (entry.State == EntityState.Added)
          userTrackable.CreatedBy = CurrentUserService.UserId.HasValue ? CurrentUserService.UserId : null;

        if (entry.State == EntityState.Modified && CurrentUserService.UserId.HasValue)
          userTrackable.UpdatedBy = CurrentUserService.UserId.HasValue ? CurrentUserService.UserId : null;
      }
    }
  }
}