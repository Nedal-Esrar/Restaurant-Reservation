using RestaurantReservation.Db;

namespace RestaurantReservation;

public class RestaurantReservationRepository
{
  private readonly RestaurantReservationDbContext _context;
  
  public RestaurantReservationRepository(RestaurantReservationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }
}