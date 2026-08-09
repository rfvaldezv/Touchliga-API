using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Communication.Commands.EditarMensaje;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Communication.EditarMensaje;

public sealed class EditarMensajeCommandHandler : IRequestHandler<EditarMensajeCommand, Unit>
{
    private readonly IMensajeRepository _mensajes;
    private readonly IUsuarioRepository _usuarios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IPushNotificationService _push;

    public EditarMensajeCommandHandler(
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

    public async Task<Unit> Handle(EditarMensajeCommand request, CancellationToken cancellationToken)
    {
        var mensaje = await _mensajes.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Mensaje");

        if (mensaje.RemitenteId != _currentUser.UserId)
            throw new BusinessException("Solo puedes editar tus propios mensajes.");

        mensaje.Editar(request.Contenido, _currentUser.UserId, request.ImagenUrl);

        _mensajes.Actualizar(mensaje);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (request.ReenviarPush)
        {
            var remitente = await _usuarios.ObtenerPorIdAsync(_currentUser.UserId);
            var textoPush = string.IsNullOrWhiteSpace(request.Contenido) ? "📷 Imagen" : request.Contenido;

            await _push.EnviarAUsuarioAsync(
                mensaje.DestinatarioId,
                remitente != null ? $"{remitente.Nombre} corrigió su mensaje" : "Mensaje corregido",
                textoPush,
                cancellationToken);
        }

        return Unit.Value;
    }
}
