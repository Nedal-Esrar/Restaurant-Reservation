using FluentValidation;
using RestaurantReservation.Api.Models.Orders;

namespace RestaurantReservation.Api.Validators.Orders;

public class OrderUpdateDtoValidator : AbstractValidator<OrderUpdateDto>
{
  public OrderUpdateDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(x => x.EmployeeId)
      .NotEmpty();

    RuleFor(x => x.ReservationId)
      .NotEmpty();

    RuleFor(x => x.TotalAmount)
      .NotEmpty()
      .GreaterThanOrEqualTo(0);
  }
}