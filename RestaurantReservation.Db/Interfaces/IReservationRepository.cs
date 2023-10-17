using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IReservationRepository : IRepository<Reservation>
{
  Task<IEnumerable<Reservation>> GetReservationsByCustomerAsync(int customerId);

  Task<IEnumerable<ReservationWithDetails>> GetReservationsWithDetailsAsync();
}