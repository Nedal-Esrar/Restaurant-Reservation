using FluentValidation;
using RestaurantReservation.Api.Models.OrderItems;

namespace RestaurantReservation.Api.Validators.OrderItems;

public class OrderItemCreationDtoValidator : AbstractValidator<OrderItemCreationDto>
{
  public OrderItemCreationDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(x => x.MenuItemId)
      .NotEmpty();

    RuleFor(x => x.Quantity)
      .NotEmpty()
      .GreaterThan(0);
  }
}