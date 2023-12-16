using AutoMapper;
using RestaurantReservation.Api.Models.Auth;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Profiles;

public class AuthProfile : Profile
{
  public AuthProfile()
  {
    CreateMap<User, UserWithoutPasswordDto>()
      .ForMember(dest => dest.Roles, options => options.MapFrom(src => src.Roles));
    CreateMap<RegisterRequestDto, User>();
    CreateMap<Role, RoleDto>();
  }
}