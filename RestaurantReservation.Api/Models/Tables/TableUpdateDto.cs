namespace RestaurantReservation.Api.Models.Tables;

public class TableUpdateDto
{
  /// <summary>
  /// The ID of the new restaurant that table belongs to.
  /// </summary>
  public int RestaurantId { get; set; }

  /// <summary>
  /// The updated capacity of the table.
  /// </summary>
  public int Capacity { get; set; }
}