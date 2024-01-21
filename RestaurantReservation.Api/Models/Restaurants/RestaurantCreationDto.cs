namespace RestaurantReservation.Api.Models.Restaurants;

public class RestaurantCreationDto
{
  /// <summary>
  /// The name of the new restaurant.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// The address of the new restaurant.
  /// </summary>
  public string Address { get; set; }

  /// <summary>
  /// The phone number of the new restaurant.
  /// </summary>
  public string PhoneNumber { get; set; }

  /// <summary>
  /// The opening hours of the new restaurant.
  /// </summary>
  public string OpeningHours { get; set; }
}