using FluentValidation;
using LifeCore.Application.Auth.Commands;

namespace LifeCore.Application.Auth.Validators;

public sealed class GoogleLoginCommandValidator : AbstractValidator<GoogleLoginCommand>
{
    public GoogleLoginCommandValidator()
    {
        RuleFor(command => command.IdToken)
            .NotEmpty();
    }
}