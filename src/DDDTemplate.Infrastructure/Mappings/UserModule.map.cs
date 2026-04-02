using DDDTemplate.Domain.Entities;
using DDDTemplate.Infrastructure.Mappings.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDTemplate.Infrastructure.Mappings;

public class UserModuleMap : BaseMap<UserModule, int>
{
  public override void Configure(EntityTypeBuilder<UserModule> builder)
  {
    base.Configure(builder);
    builder.Property(c => c.ModuleId).IsRequired();
    builder.Property(c => c.Insert).HasDefaultValue(false);
    builder.Property(c => c.Update).HasDefaultValue(false);
    builder.Property(c => c.Delete).HasDefaultValue(false);

    builder.HasOne(c => c.User).WithMany(c => c.UserModules).HasForeignKey(c => c.UserId).IsRequired();
    builder.HasOne(c => c.Module).WithMany().HasForeignKey(c => c.ModuleId).IsRequired();
  }
}