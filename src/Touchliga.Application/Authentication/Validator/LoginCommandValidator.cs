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

        // Sin mínimo de longitud aquí -- por consistencia con FutLiga,
        // donde algunos participantes migrados heredaron contraseñas
        // cortas. El mínimo de 6 solo aplica al crear/restablecer.
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
