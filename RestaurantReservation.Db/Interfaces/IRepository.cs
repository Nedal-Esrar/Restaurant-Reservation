using System.Linq.Expressions;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
  Task<(IEnumerable<TEntity>, PaginationMetadata)> GetAllAsync(Expression<Func<TEntity, bool>> filter,
    int pageNumber, int pageSize);
  
  Task<TEntity> CreateAsync(TEntity entity);

  Task UpdateAsync(TEntity entity);

  Task DeleteAsync(int id);

  Task<bool> IsExistAsync(int id);
}