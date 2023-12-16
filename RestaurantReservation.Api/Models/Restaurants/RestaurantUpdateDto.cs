namespace RestaurantReservation.Api.Models.Restaurants;

public class RestaurantUpdateDto
{
  /// <summary>
  /// The updated name of the restaurant.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// The updated address of the restaurant.
  /// </summary>
  public string Address { get; set; }

  /// <summary>
  /// The updated phone number of the restaurant.
  /// </summary>
  public string PhoneNumber { get; set; }

  /// <summary>
  /// The updated opening hours of the restaurant.
  /// </summary>
  public string OpeningHours { get; set; }
}