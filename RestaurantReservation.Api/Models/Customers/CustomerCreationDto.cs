namespace RestaurantReservation.Api.Models.Customers;

public class CustomerCreationDto
{
  /// <summary>
  /// The first name of the new customer
  /// </summary>
  public string FirstName { get; set; }

  /// <summary>
  /// The last name of the new customer
  /// </summary>
  public string LastName { get; set; }

  /// <summary>
  /// The email of the new customer
  /// </summary>
  public string Email { get; set; }

  /// <summary>
  /// The phone number of the new customer
  /// </summary>
  public string PhoneNumber { get; set; }
}