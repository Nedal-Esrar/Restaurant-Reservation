namespace RestaurantReservation.Api.Models.Restaurants;

public class RestaurantResponseDto
{
  public int Id { get; set; }
  
  public string Name { get; set; }

  public string Address { get; set; }

  public string PhoneNumber { get; set; }

  public string OpeningHours { get; set; }
}