using MediatR;

namespace Touchliga.Application.Communication.Commands.EnviarMensaje;

/// <summary>El remitente siempre es el usuario autenticado. Contenido
/// puede ir vacío si se manda ImagenUrl (mensaje solo-imagen).</summary>
public sealed record EnviarMensajeCommand(long DestinatarioId, string Contenido, string? ImagenUrl = null)
    : IRequest<long>;
