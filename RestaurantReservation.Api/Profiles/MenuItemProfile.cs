using AutoMapper;
using RestaurantReservation.Api.Models.MenuItems;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Profiles;

public class MenuItemProfile : Profile
{
  public MenuItemProfile()
  {
    CreateMap<MenuItem, MenuItemResponseDto>();
    CreateMap<MenuItemCreationDto, MenuItem>();
    CreateMap<MenuItemUpdateDto, MenuItem>().ReverseMap();
  }
}