using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IReservationRepository
{
  Task CreateAsync(Reservation reservation);

  Task UpdateAsync(Reservation reservation);

  Task<bool> IsExistAsync(int id);

  Task DeleteAsync(int id);

  Task<IEnumerable<Reservation>> GetReservationsByCustomerAsync(int customerId);

  Task<IEnumerable<ReservationWithDetails>> GetReservationsWithDetailsAsync();
}