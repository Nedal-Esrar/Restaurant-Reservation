namespace RestaurantReservation.Api.Models.Tables;

public class TableResponseDto
{
  /// <summary>
  /// The ID of the table.
  /// </summary>
  public int Id { get; set; }
  
  /// <summary>
  /// The ID of the restaurant that table belongs to.
  /// </summary>
  public int? RestaurantId { get; set; }

  /// <summary>
  /// The capacity of the table.
  /// </summary>
  public int Capacity { get; set; }
}