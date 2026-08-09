using MediatR;

namespace Touchliga.Application.Users.Commands.RestablecerPassword;

/// <summary>Soporte: genera una contraseña temporal nueva para un
/// participante y regresa la contraseña en texto plano UNA sola vez,
/// para que el admin se la comparta.</summary>
public sealed record RestablecerPasswordCommand(long UsuarioId) : IRequest<string>;
