using AutoMapper;
using RestaurantReservation.Api.Models.Customers;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Profiles;

public class CustomerProfile : Profile
{
  public CustomerProfile()
  {
    CreateMap<Customer, CustomerResponseDto>();
    CreateMap<CustomerCreationDto, Customer>();
    CreateMap<CustomerUpdateDto, Customer>().ReverseMap();
  }
}