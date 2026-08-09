using MediatR;

namespace Touchliga.Application.Communication.Commands.EliminarMensaje;

public sealed record EliminarMensajeCommand(long Id) : IRequest<Unit>;
