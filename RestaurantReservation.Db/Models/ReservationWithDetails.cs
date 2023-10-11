namespace RestaurantReservation.Db.Models;

public class ReservationWithDetails
{
  public int ReservationId { get; set; }
  
  public int? ReservationTableId { get; set; }
  
  public DateTime ReservationDate { get; set; }
  
  public int PartySize { get; set; }
  
  public int CustomerId { get; set; }
  
  public string CustomerFirstName { get; set; }
  
  public string CustomerLastName { get; set; }
  
  public string CustomerEmail { get; set; }
  
  public string CustomerPhoneNumber { get; set; }
  
  public int RestaurantId { get; set; }
  
  public string RestaurantName { get; set; }
  
  public string RestaurantAddress { get; set; }
  
  public string RestaurantPhoneNumber { get; set; }
  
  public string RestaurantOpeningHours { get; set; }
}