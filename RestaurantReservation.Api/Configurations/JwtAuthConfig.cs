namespace RestaurantReservation.Api.Configurations;

public class JwtAuthConfig
{
  public string Key { get; set; } = string.Empty;

  public string Issuer { get; set; } = string.Empty;

  public string Audience { get; set; } = string.Empty;

  public double LifetimeMinutes { get; set; }
}