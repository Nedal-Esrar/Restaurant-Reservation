namespace RestaurantReservation.Api.Models.Customers;

public class CustomerUpdateDto
{
  /// <summary>
  /// The updated value of the first name of the customer
  /// </summary>
  public string FirstName { get; set; }

  /// <summary>
  /// The updated value of the last name of the customer
  /// </summary>
  public string LastName { get; set; }

  /// <summary>
  /// The updated value of the email of the customer
  /// </summary>
  public string Email { get; set; }

  /// <summary>
  /// The updated value of the phone number of the customer
  /// </summary>
  public string PhoneNumber { get; set; }
}