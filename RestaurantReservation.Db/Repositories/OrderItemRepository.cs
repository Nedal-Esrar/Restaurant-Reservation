using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories;

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
  public OrderItemRepository(RestaurantReservationDbContext context) : base(context)
  {
  }
}