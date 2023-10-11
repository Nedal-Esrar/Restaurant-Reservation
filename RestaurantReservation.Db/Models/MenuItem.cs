namespace RestaurantReservation.Db.Models;

public class MenuItem : Entity
{
  public int? RestaurantId { get; set; }

  public Restaurant Restaurant { get; set; }

  public string Name { get; set; }

  public string Description { get; set; }

  public decimal Price { get; set; }

  public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}