using System.Linq.Expressions;
using DDDTemplate.Domain.Interfaces.Entities;
using DDDTemplate.Domain.Interfaces.Repositories.Base;
using DDDTemplate.Domain.Interfaces.Services.Base;

namespace DDDTemplate.Domain.Services.Base;

public abstract class BaseService<TEntity, TId>(IBaseRepository<TEntity, TId> repository) : IBaseService<TEntity, TId> where TEntity : IEntity<TId>
{
  protected readonly IBaseRepository<TEntity, TId> repository = repository;

  public virtual IEnumerable<TEntity> GetAll(
    Expression<Func<TEntity, bool>>? filter = null,
    int? skip = null,
    int? take = null,
    Expression<Func<TEntity, object>>? orderByField = null,
    string orderByDirection = "DESC"
  )
  {
    return repository.GetAll(filter, skip, take, orderByField, orderByDirection);
  }

  public virtual int Count(Expression<Func<TEntity, bool>>? filter = null)
  {
    return repository.Count(filter);
  }

  public virtual TEntity GetById(TId id)
  {
    return repository.GetById(id);
  }

  public virtual TId Insert(TEntity entity)
  {
    return repository.Insert(entity);
  }

  public virtual void BulkInsert(ICollection<TEntity> entities)
  {
    repository.BulkInsert(entities);
  }

  public virtual void Update(TEntity entity)
  {
    repository.Update(entity);
  }

  public virtual void BulkUpdate(ICollection<TEntity> entities)
  {
    repository.BulkUpdate(entities);
  }

  public virtual void Delete(TEntity entity)
  {
    repository.Delete(entity);
  }
}