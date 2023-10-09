namespace RestaurantReservation.Db;

public static class StandardMessages
{
  public static string GenerateNotFoundMessage(string name, int id) =>
    $"{name} with id {id} does not exist";
}