using MediatR;

namespace Touchliga.Application.Users.Commands.AsignarRol;

public sealed record AsignarRolCommand(long UsuarioId, long RolId) : IRequest<Unit>;
