using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
  Task<IEnumerable<Customer>> FindCustomersWithPartySizeLargerThanAsync(int minPartySize);
}