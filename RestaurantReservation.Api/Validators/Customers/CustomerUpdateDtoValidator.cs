using FluentValidation;
using RestaurantReservation.Api.Models.Customers;

namespace RestaurantReservation.Api.Validators.Customers;

public class CustomerUpdateDtoValidator : AbstractValidator<CustomerUpdateDto>
{
  public CustomerUpdateDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(x => x.FirstName)
      .NotEmpty()
      .ValidName();
    
    RuleFor(x => x.LastName)
      .NotEmpty()
      .ValidName();
    
    RuleFor(x => x.Email)
      .NotEmpty()
      .EmailAddress();
    
    RuleFor(x => x.PhoneNumber)
      .NotEmpty()
      .ValidPhoneNumber();
  }
}