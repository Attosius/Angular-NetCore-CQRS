using FluentValidation;
using PromomashInc.EntitiesDto.Command;

namespace PromomashInc.Server.Handlers;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Please specify a valid email address");

        RuleFor(x => x.Password)
            .Matches("^(?=.*[0-9])(?=.*[A-Z]).+$")
            .WithMessage("Password must contains min 1 digit and min 1 uppercase letter");

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage("CountryCode cannot be empty");

        RuleFor(x => x.ProvinceCode)
            .NotEmpty()
            .WithMessage("ProvinceCode cannot be empty");
    }
}