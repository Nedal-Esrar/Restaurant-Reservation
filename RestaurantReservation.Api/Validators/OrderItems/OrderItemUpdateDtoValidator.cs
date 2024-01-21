using FluentValidation;
using RestaurantReservation.Api.Models.OrderItems;

namespace RestaurantReservation.Api.Validators.OrderItems;

public class OrderItemUpdateDtoValidator : AbstractValidator<OrderItemUpdateDto>
{
  public OrderItemUpdateDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
      
    RuleFor(x => x.Quantity)
      .NotEmpty()
      .GreaterThan(0);
  }
}