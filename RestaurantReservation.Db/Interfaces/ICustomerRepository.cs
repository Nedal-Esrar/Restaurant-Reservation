using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface ICustomerRepository
{
  Task<IEnumerable<Customer>> FindCustomersWithPartySizeLargerThanAsync(int minPartySize);
}