using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces;

public interface IReservationRepository : IRepository<Reservation>
{
  Task<IEnumerable<Reservation>> GetReservationsByCustomerAsync(int customerId);

  Task<IEnumerable<ReservationWithDetails>> GetReservationsWithDetailsAsync();
}