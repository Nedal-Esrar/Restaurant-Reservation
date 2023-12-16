using FluentValidation;
using RestaurantReservation.Api.Models.Tables;

namespace RestaurantReservation.Api.Validators.Tables;

public class TableUpdateDtoValidator : AbstractValidator<TableUpdateDto>
{
  public TableUpdateDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(x => x.RestaurantId)
      .NotEmpty();

    RuleFor(x => x.Capacity)
      .NotEmpty()
      .InclusiveBetween(1, 10);
  }
}