using BikeRent.Exception.ExceptionBase;
using FluentValidation;

namespace BikeDeliveryMan.Application.UseCase;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password).NotEmpty().WithMessage(ErrorsMessages.BlankPassword);
        When(password => !string.IsNullOrWhiteSpace(password), () =>
        {
            RuleFor(password => password.Length).GreaterThanOrEqualTo(6).WithMessage(ErrorsMessages.MinimumSixCharacters);
        });
    }
}