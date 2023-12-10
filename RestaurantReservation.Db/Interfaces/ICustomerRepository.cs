using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
  Task<IEnumerable<Customer>> FindCustomersWithPartySizeLargerThanAsync(int minPartySize);
}