using FluentValidation;
using RestaurantReservation.Api.Models.Auth;

namespace RestaurantReservation.Api.Validators.Auth;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
  public LoginRequestDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;

    RuleFor(x => x.Username)
      .NotEmpty()
      .ValidName();

    RuleFor(x => x.Password)
      .NotEmpty()
      .StrongPassword();
  }
}