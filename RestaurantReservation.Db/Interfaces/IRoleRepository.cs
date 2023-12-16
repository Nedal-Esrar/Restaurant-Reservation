using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces;

public interface IRoleRepository
{
  Task<Role?> GetByName(string name);
}