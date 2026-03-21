using DDDTemplate.Domain.Entities;
using DDDTemplate.Infrastructure.Mappings.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDTemplate.Infrastructure.Mappings;

public class UserMap : CommonMap<User, Guid>
{
  public override void Configure(EntityTypeBuilder<User> builder)
  {
    base.Configure(builder);
    builder.Property(c => c.Name).IsRequired();
  }
}