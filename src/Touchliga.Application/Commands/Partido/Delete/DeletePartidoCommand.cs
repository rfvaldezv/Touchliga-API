using MediatR;

namespace Touchliga.Application.Commands.Partido.Delete;

public sealed record DeletePartidoCommand(long Id) : IRequest<Unit>;
