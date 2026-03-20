using System.Linq.Expressions;
using DDDTemplate.Domain.Interfaces.Entities;

namespace DDDTemplate.Domain.Interfaces.Services.Base;

public interface IBaseService<TEntity, TId> where TEntity : IEntity<TId>
{
  IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, int? skip = null, int? take = null, Expression<Func<TEntity, object>>? orderByField = null, string orderByDirection = "DESC");
  int Count(Expression<Func<TEntity, bool>>? filter = null);
  TEntity GetById(TId id);
  TId Insert(TEntity entity);
  void BulkInsert(ICollection<TEntity> entities);
  void Update(TEntity entity);
  void BulkUpdate(ICollection<TEntity> entities);
  void Delete(TEntity entity);
}