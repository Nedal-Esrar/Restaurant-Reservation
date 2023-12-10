using FluentValidation;
using RestaurantReservation.Api.Models.MenuItems;

namespace RestaurantReservation.Api.Validators.MenuItems;

public class MenuItemUpdateDtoValidator : AbstractValidator<MenuItemUpdateDto>
{
  public MenuItemUpdateDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(x => x.Name)
      .NotEmpty()
      .ValidName();

    RuleFor(x => x.Description)
      .NotEmpty();

    RuleFor(x => x.Price)
      .NotEmpty()
      .GreaterThan(0);

    RuleFor(x => x.RestaurantId)
      .NotEmpty();
  }
}