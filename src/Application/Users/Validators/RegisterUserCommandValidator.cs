using FluentValidation;
using LifeOS.Application.Users.Commands;

namespace LifeOS.Application.Users.Validators;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.FullName)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(100);
    }
}