using MediatR;

namespace Touchliga.Application.Users.Commands.QuitarRol;

public sealed record QuitarRolCommand(long UsuarioId, long RolId) : IRequest<Unit>;
