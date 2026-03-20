using System.Linq.Expressions;
using EFCore.BulkExtensions;
using DDDTemplate.Domain.Interfaces.Entities;
using DDDTemplate.Domain.Interfaces.Repositories.Base;
using DDDTemplate.Infrastructure.Contexts;

namespace DDDTemplate.Infrastructure.Repositories.Base;

public abstract class BaseRepository<TEntity, TId>(DatabaseContext databaseContext) : IBaseRepository<TEntity, TId> where TEntity : class, IEntity<TId>
{
  protected readonly DatabaseContext DatabaseContext = databaseContext;

  private IQueryable<TEntity> CreateQuery(
    Expression<Func<TEntity, bool>>? filter = null,
    int? skip = null,
    int? take = null,
    Expression<Func<TEntity, object>>? orderByField = null,
    string orderByDirection = "DESC"
  )
  {
    IQueryable<TEntity> query = DatabaseContext.Set<TEntity>();

    if (filter is not null)
      query = query.Where(filter);

    if (orderByField is not null)
      query = orderByDirection == "DESC" ? query.OrderByDescending(orderByField) : query.OrderBy(orderByField);
    else
      query = query.OrderByDescending(c => c.Id);

    if (skip.HasValue)
      query = query.Skip(skip.Value);

    if (take.HasValue)
      query = query.Take(take.Value);

    return query;
  }

  private IQueryable<TEntity> CreateCountQuery(Expression<Func<TEntity, bool>>? filter = null)
  {
    IQueryable<TEntity> query = DatabaseContext.Set<TEntity>();

    if (filter is not null)
      query = query.Where(filter);

    return query;
  }

  public virtual IEnumerable<TEntity> GetAll(
    Expression<Func<TEntity, bool>>? filter = null,
    int? skip = null,
    int? take = null,
    Expression<Func<TEntity, object>>? orderByField = null,
    string orderByDirection = "DESC"
  )
  {
    return CreateQuery(filter, skip, take, orderByField, orderByDirection).ToList();
  }

  public virtual int Count(Expression<Func<TEntity, bool>>? filter = null)
  {
    return CreateCountQuery(filter).Count();
  }

  public virtual TEntity GetById(TId id)
  {
    return DatabaseContext.Set<TEntity>().Find(id)!;
  }

  public virtual TId Insert(TEntity entity)
  {
    DatabaseContext.Set<TEntity>().Add(entity);
    DatabaseContext.SaveChanges();

    return entity.Id;
  }

  public virtual void BulkInsert(ICollection<TEntity> entities)
  {
    DatabaseContext.BulkInsert(entities);
    DatabaseContext.BulkSaveChanges();
  }

  public virtual void Update(TEntity entity)
  {
    DatabaseContext.Set<TEntity>().Update(entity);
    DatabaseContext.SaveChanges();
  }

  public virtual void BulkUpdate(ICollection<TEntity> entities)
  {
    DatabaseContext.BulkUpdate(entities);
    DatabaseContext.BulkSaveChanges();
  }

  public virtual void Delete(TEntity entity)
  {
    DatabaseContext.Set<TEntity>().Remove(entity);
    DatabaseContext.SaveChanges();
  }
}
