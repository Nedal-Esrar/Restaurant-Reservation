using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IReservationRepository
{
  Task<IEnumerable<Reservation>> GetReservationsByCustomerAsync(int customerId);

  Task<IEnumerable<ReservationWithDetails>> GetReservationsWithDetailsAsync();
}