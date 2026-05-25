using FluentValidation;
using LifeOS.Application.Auth.Commands;

namespace LifeOS.Application.Auth.Validators;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(command => command.RefreshToken)
            .NotEmpty();
    }
}