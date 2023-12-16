using FluentValidation;
using RestaurantReservation.Api.Models.Tables;

namespace RestaurantReservation.Api.Validators.Tables;

public class TableCreationDtoValidator : AbstractValidator<TableCreationDto>
{
  public TableCreationDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
      
    RuleFor(x => x.RestaurantId)
      .NotEmpty();

    RuleFor(x => x.Capacity)
      .NotEmpty()
      .InclusiveBetween(1, 10);
  }
}