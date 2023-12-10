using FluentValidation;
using RestaurantReservation.Api.Models.Reservations;

namespace RestaurantReservation.Api.Validators.Reservations;

public class ReservationCreationDtoValidator : AbstractValidator<ReservationCreationDto>
{
  public ReservationCreationDtoValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(x => x.RestaurantId)
      .NotEmpty();

    RuleFor(x => x.CustomerId)
      .NotEmpty();

    RuleFor(x => x.TableId)
      .NotEmpty();

    RuleFor(x => x.PartySize)
      .NotEmpty()
      .InclusiveBetween(1, 10);

    RuleFor(x => x.ReservationDate)
      .NotEmpty();
  }
}