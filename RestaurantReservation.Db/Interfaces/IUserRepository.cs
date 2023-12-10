using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces;

public interface IUserRepository
{
  Task<User?> AuthenticateAsync(string email, string password);

  Task<bool> IsExistByUsernameAsync(string username);

  Task<User> CreateAsync(User user);
}