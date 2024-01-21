using AutoMapper;
using RestaurantReservation.Api.Models.Employees;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Profiles;

public class EmployeeProfile : Profile
{
  public EmployeeProfile()
  {
    CreateMap<Employee, EmployeeResponseDto>();
    CreateMap<EmployeeCreationDto, Employee>();
    CreateMap<EmployeeUpdateDto, Employee>().ReverseMap();
  }
}