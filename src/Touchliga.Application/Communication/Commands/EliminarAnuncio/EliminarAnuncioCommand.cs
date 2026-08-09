using MediatR;

namespace Touchliga.Application.Communication.Commands.EliminarAnuncio;

public sealed record EliminarAnuncioCommand(long Id) : IRequest<Unit>;
