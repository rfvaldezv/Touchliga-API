using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Entities;
using Touchliga.Application.Communication.Commands.EnviarMensaje;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Communication.EnviarMensaje;

public sealed class EnviarMensajeCommandHandler : IRequestHandler<EnviarMensajeCommand, long>
{
    private readonly IMensajeRepository _mensajes;
    private readonly IUsuarioRepository _usuarios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IPushNotificationService _push;

    public EnviarMensajeCommandHandler(
        IMensajeRepository mensajes,
        IUsuarioRepository usuarios,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IPushNotificationService push)
    {
        _mensajes = mensajes;
        _usuarios = usuarios;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _push = push;
    }

    public async Task<long> Handle(EnviarMensajeCommand request, CancellationToken cancellationToken)
    {
        var mensaje = Mensaje.Crear(_currentUser.UserId, request.DestinatarioId, request.Contenido, request.ImagenUrl);

        await _mensajes.AgregarAsync(mensaje, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var remitente = await _usuarios.ObtenerPorIdAsync(_currentUser.UserId);
        var textoPush = string.IsNullOrWhiteSpace(request.Contenido) ? "📷 Imagen" : request.Contenido;

        await _push.EnviarAUsuarioAsync(
            request.DestinatarioId,
            remitente != null ? $"{remitente.Nombre} te escribió" : "Nuevo mensaje",
            textoPush,
            cancellationToken);

        return mensaje.Id;
    }
}
