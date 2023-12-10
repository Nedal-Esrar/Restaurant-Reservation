namespace RestaurantReservation.Api.Validators;

public class ValidationMessages
{
  public const string ValidName = "'{PropertyName}' should only contain letters and spaces.";
  
  public const string PasswordUpperCase = "Password must include upper case letters.";

  public const string PasswordLowerCase = "Password must include lower case letters.";

  public const string PasswordDigits = "Password must include digits";

  public const string PasswordSpecialCharacters = "Password must include special characters";
  
  public const string ValidPhoneNumber = "'{PropertyName}' Must be a valid phone number.";
}