using MediatR;

namespace Touchliga.Application.Communication.Commands.EditarMensaje;

public sealed record EditarMensajeCommand(long Id, string Contenido, bool ReenviarPush, string? ImagenUrl = null)
    : IRequest<Unit>;
