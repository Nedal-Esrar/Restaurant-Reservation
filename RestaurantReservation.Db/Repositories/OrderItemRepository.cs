using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class OrderItemRepository : Repository<OrderItem>
{
  public OrderItemRepository(RestaurantReservationDbContext context) : base(context)
  {
  }
}