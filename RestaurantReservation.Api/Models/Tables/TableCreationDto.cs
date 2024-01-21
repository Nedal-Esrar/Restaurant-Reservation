namespace RestaurantReservation.Api.Models.Tables;

public class TableCreationDto
{
  /// <summary>
  /// The ID of the restaurant of the newly created table.
  /// </summary>
  public int RestaurantId { get; set; }

  /// <summary>
  /// The capacity of the new table.
  /// </summary>
  public int Capacity { get; set; }
}