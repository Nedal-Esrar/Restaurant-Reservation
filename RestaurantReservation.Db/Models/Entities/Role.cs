namespace RestaurantReservation.Db.Models.Entities;

public class Role : Entity
{
  public string Name { get; set; }

  public ICollection<User> Users { get; set; } = new List<User>();
}