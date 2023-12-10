using FluentValidation;

namespace RestaurantReservation.Api.Validators;

public static class ValidationExtensions
{
  public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.MinimumLength(8)
      .Matches("[A-Z]").WithMessage(ValidationMessages.PasswordUpperCase)
      .Matches("[a-z]").WithMessage(ValidationMessages.PasswordLowerCase)
      .Matches("[0-9]").WithMessage(ValidationMessages.PasswordDigits)
      .Matches("[^a-zA-Z0-9]").WithMessage(ValidationMessages.PasswordSpecialCharacters);
  }
}