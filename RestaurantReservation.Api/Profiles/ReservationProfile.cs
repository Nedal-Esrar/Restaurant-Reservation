using AutoMapper;
using RestaurantReservation.Api.Models.Reservations;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Profiles;

public class ReservationProfile : Profile
{
  public ReservationProfile()
  {
    CreateMap<Reservation, ReservationResponseDto>();
    CreateMap<ReservationCreationDto, Reservation>();
    CreateMap<ReservationUpdateDto, Reservation>().ReverseMap();
  }
}