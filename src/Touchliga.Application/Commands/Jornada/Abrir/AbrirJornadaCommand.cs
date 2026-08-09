using MediatR;

namespace Touchliga.Application.Commands.Jornada.Abrir;

public sealed record AbrirJornadaCommand(long Id) : IRequest<Unit>;
