using RestaurantReservation.Api.Auth;
using RestaurantReservation.Api.Models.Auth;

namespace RestaurantReservation.Api.Interfaces;

public interface IJwtTokenGenerator
{
  JwtToken GenerateToken(UserWithoutPasswordDto user);
}