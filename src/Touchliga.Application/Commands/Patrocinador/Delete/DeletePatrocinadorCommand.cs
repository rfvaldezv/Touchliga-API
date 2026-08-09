using MediatR;

namespace Touchliga.Application.Commands.Patrocinador.Delete;

public sealed record DeletePatrocinadorCommand(long Id) : IRequest<Unit>;
