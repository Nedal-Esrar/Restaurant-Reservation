using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface ITableRepository
{
  Task CreateAsync(Table table);

  Task UpdateAsync(Table table);

  Task<bool> IsExistAsync(int id);

  Task DeleteAsync(int id);
}