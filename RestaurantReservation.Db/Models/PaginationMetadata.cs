namespace RestaurantReservation.Db.Models;

public class PaginationMetadata
{
  public int TotalItemCount { get; set; }

  public int TotalPageCount => (int)Math.Ceiling((double)TotalItemCount / PageSize);

  public int PageSize { get; set; }

  public int CurrentPage { get; set; }
}