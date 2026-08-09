using MediatR;

namespace Touchliga.Application.Users.Commands.CambiarMiPassword;

/// <summary>El propio participante cambia su contraseña desde Perfil
/// (a diferencia de RestablecerPassword, que usa el admin para
/// soporte y genera una temporal aleatoria).</summary>
public sealed record CambiarMiPasswordCommand(string PasswordActual, string PasswordNueva) : IRequest<Unit>;
