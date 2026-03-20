using DDDTemplate.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDTemplate.Infrastructure.Mappings.Base;

public abstract class EnumMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EnumEntity
{
  public virtual void Configure(EntityTypeBuilder<TEntity> builder)
  {
    builder.HasKey(c => c.Id);
    builder.Property(c => c.Description).IsRequired();
  }
}
