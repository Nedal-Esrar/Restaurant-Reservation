using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
  Task<TEntity> CreateAsync(TEntity entity);

  Task UpdateAsync(TEntity entity);

  Task DeleteAsync(int id);

  Task<bool> IsExistAsync(int id);
}