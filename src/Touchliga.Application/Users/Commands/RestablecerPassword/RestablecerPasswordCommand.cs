using MediatR;

namespace Touchliga.Application.Users.Commands.RestablecerPassword;

/// <summary>NuevaPassword es opcional: si el admin escribe una
/// específica se usa esa, si no se genera una aleatoria.</summary>
public sealed record RestablecerPasswordCommand(long UsuarioId, string? NuevaPassword = null) : IRequest<string>;
