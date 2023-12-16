using AutoMapper;
using RestaurantReservation.Api.Models.Tables;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Profiles;

public class TableProfile : Profile
{
  public TableProfile()
  {
    CreateMap<Table, TableResponseDto>();
    CreateMap<TableCreationDto, Table>();
    CreateMap<TableUpdateDto, Table>().ReverseMap();
  }
}