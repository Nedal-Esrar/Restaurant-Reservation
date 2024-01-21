using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.Api.Configurations;
using RestaurantReservation.Api.Interfaces;
using RestaurantReservation.Api.Models.Auth;

namespace RestaurantReservation.Api.Auth;

public class JwtTokenGenerator : IJwtTokenGenerator
{
  private readonly JwtAuthConfig _jwtAuthConfig;

  public JwtTokenGenerator(IOptions<JwtAuthConfig> jwtAuthConfig)
  {
    _jwtAuthConfig = jwtAuthConfig.Value;
  }
  
  public JwtToken GenerateToken(UserWithoutPasswordDto user)
  {
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthConfig.Key));

    var claims = new List<Claim>
    {
      new(ClaimTypes.Name, user.Username)
    };
    
    claims.AddRange(user.Roles.Select(userRole => new Claim(ClaimTypes.Role, userRole.Name)));
    
    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    
    var jwtSecurityToken = new JwtSecurityToken(
      issuer: _jwtAuthConfig.Issuer,
      audience: _jwtAuthConfig.Audience,
      claims: claims,
      notBefore: DateTime.UtcNow,
      expires: DateTime.UtcNow.AddMinutes(_jwtAuthConfig.LifetimeMinutes),
      signingCredentials: signingCredentials
    );
    
    var token = new JwtSecurityTokenHandler()
      .WriteToken(jwtSecurityToken);

    return new JwtToken { Token = token };
  }
}