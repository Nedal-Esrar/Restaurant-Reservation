using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
  public OrderItemRepository(RestaurantReservationDbContext context) : base(context)
  {
  }
}