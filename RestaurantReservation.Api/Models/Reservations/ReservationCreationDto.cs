namespace RestaurantReservation.Api.Models.Reservations;

public class ReservationCreationDto
{
  /// <summary>
  /// The ID of the customer of the new reservation.
  /// </summary>
  public int CustomerId { get; set; }

  /// <summary>
  /// The ID of the restaurant of the new reservation.
  /// </summary>
  public int RestaurantId { get; set; }

  /// <summary>
  /// The ID of the table of the new reservation.
  /// </summary>
  public int TableId { get; set; }

  /// <summary>
  /// The Date when the new reservation was made.
  /// </summary>
  public DateTime ReservationDate { get; set; }

  /// <summary>
  /// The party size of the new reservation.
  /// </summary>
  public int PartySize { get; set; }
}