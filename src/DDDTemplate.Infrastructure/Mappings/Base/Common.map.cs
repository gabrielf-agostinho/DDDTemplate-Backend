using DDDTemplate.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDTemplate.Infrastructure.Mappings.Base;

public abstract class CommonMap<TEntity, TId> : IEntityTypeConfiguration<TEntity> where TEntity : CommonEntity<TId>
{
  public virtual void Configure(EntityTypeBuilder<TEntity> builder)
  {
    builder.HasKey(c => c.Id);
    builder.Property(c => c.IsActive).HasDefaultValue(true);
    builder.Property(c => c.IsDeleted).HasDefaultValue(false);
  }
}