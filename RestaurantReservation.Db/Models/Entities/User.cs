namespace RestaurantReservation.Db.Models.Entities;

public class User : Entity
{
  public string Username { get; set; }
  
  public string Password { get; set; }

  public ICollection<Role> Roles { get; set; } = new List<Role>();
}