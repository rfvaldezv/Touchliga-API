using FluentValidation;

namespace Touchliga.Application.Authentication.Commands.Login;

public sealed class LoginCommandValidator
    : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Correo)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}
