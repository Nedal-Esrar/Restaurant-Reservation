using FluentValidation;
using RestaurantReservation.Api.Models.Employees;

namespace RestaurantReservation.Api.Validators.Employees;

public class EmployeeUpdateDtoValidator : AbstractValidator<EmployeeUpdateDto>
{
  public EmployeeUpdateDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(x => x.RestaurantId)
      .NotEmpty();

    RuleFor(x => x.FirstName)
      .NotEmpty()
      .ValidName();
    
    RuleFor(x => x.LastName)
      .NotEmpty()
      .ValidName();

    RuleFor(x => x.Position)
      .NotEmpty()
      .IsInEnum();
  }
}