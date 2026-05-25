using FluentValidation;
using LifeOS.Application.Auth.Commands;

namespace LifeOS.Application.Auth.Validators;

public sealed class GoogleLoginCommandValidator : AbstractValidator<GoogleLoginCommand>
{
    public GoogleLoginCommandValidator()
    {
        RuleFor(command => command.IdToken)
            .NotEmpty();
    }
}