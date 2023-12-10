using FluentValidation;
using RestaurantReservation.Api.Models.Orders;

namespace RestaurantReservation.Api.Validators.Orders;

public class OrderCreationDtoValidator : AbstractValidator<OrderCreationDto>
{
  public OrderCreationDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(x => x.EmployeeId)
      .NotEmpty();

    RuleFor(x => x.OrderDate)
      .NotEmpty();

    RuleFor(x => x.ReservationId)
      .NotEmpty();

    RuleFor(x => x.TotalAmount)
      .NotEmpty()
      .GreaterThanOrEqualTo(0);
  }
}