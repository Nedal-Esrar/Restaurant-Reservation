namespace RestaurantReservation.Api.Models.Reservations;

public class ReservationUpdateDto
{
  /// <summary>
  /// The ID of the updated customer that made the reservation.
  /// </summary>
  public int CustomerId { get; set; }

  /// <summary>
  /// The ID of the updated restaurant for the reservation.
  /// </summary>
  public int RestaurantId { get; set; }

  /// <summary>
  /// The ID of the updated table for the reservation.
  /// </summary>
  public int TableId { get; set; }

  /// <summary>
  /// The updated party size of the reservation.
  /// </summary>
  public int PartySize { get; set; }
}