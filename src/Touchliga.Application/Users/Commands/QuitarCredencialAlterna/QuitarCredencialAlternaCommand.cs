using MediatR;

namespace Touchliga.Application.Users.Commands.QuitarCredencialAlterna;

public sealed record QuitarCredencialAlternaCommand(long UsuarioId) : IRequest<Unit>;
