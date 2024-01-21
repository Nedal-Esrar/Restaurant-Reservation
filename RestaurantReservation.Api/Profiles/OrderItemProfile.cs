using AutoMapper;
using RestaurantReservation.Api.Models.OrderItems;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Profiles;

public class OrderItemProfile : Profile
{
  public OrderItemProfile()
  {
    CreateMap<OrderItem, OrderItemResponseDto>();
    CreateMap<OrderItemCreationDto, OrderItem>();
    CreateMap<OrderItemUpdateDto, OrderItem>().ReverseMap();
  }
}