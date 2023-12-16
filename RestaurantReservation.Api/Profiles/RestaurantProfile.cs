using AutoMapper;
using RestaurantReservation.Api.Models.Restaurants;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Profiles;

public class RestaurantProfile : Profile
{
  public RestaurantProfile()
  {
    CreateMap<Restaurant, RestaurantResponseDto>();
    CreateMap<RestaurantCreationDto, Restaurant>();
    CreateMap<RestaurantUpdateDto, Restaurant>().ReverseMap();
  }
}