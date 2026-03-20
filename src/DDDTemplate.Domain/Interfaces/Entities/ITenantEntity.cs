namespace DDDTemplate.Domain.Interfaces.Entities;

public interface ITenantEntity<TId>
{
  TId TenantId { get; set; }
}