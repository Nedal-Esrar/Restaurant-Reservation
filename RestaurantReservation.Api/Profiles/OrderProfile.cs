using AutoMapper;
using RestaurantReservation.Api.Models.Orders;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Profiles;

public class OrderProfile : Profile
{
  public OrderProfile()
  {
    CreateMap<Order, OrderResponseDto>();
    CreateMap<OrderCreationDto, Order>();
    CreateMap<OrderUpdateDto, Order>().ReverseMap();
    
    CreateMap<Order, OrderWithMenuItemsResponseDto>()
      .ForMember(dest => dest.MenuItems,
        options => options.MapFrom(src => src.OrderItems.Select(oi => oi.Item)));

  }
}