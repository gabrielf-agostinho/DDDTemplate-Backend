using System.Linq.Expressions;
using DDDTemplate.Application.Utils;
using DDDTemplate.Domain.Interfaces.Entities;

namespace DDDTemplate.Application.Interfaces.Base;

public interface IBaseAppService<TEntity, TId, TGetDTO, TPostDTO, TPutDTO> where TEntity : IEntity<TId>
{
  IEnumerable<TGetDTO> GetAll();
  Pagination<TGetDTO> GetPaged(int? skip, int? take, Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? orderByField = null, string orderByDirection = "DESC");
  TGetDTO GetById(TId id);
  TId Insert(TPostDTO dto);
  void Update(TPutDTO dto);
  void Update(TPutDTO dto, TEntity entity);
  void Delete(TId id);
}
