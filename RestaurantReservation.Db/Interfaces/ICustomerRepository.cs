using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface ICustomerRepository
{
  Task CreateAsync(Customer customer);

  Task UpdateAsync(Customer customer);

  Task DeleteAsync(int id);

  Task<bool> IsExistAsync(int id);

  Task<IEnumerable<Customer>> GetCustomersWithLargePartiesAsync(int minPartySize);
}