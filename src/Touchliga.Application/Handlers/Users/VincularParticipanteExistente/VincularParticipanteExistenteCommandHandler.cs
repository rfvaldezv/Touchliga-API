using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Entities;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Users.Commands.VincularParticipanteExistente;

namespace Touchliga.Application.Handlers.Users.VincularParticipanteExistente;

public sealed class VincularParticipanteExistenteCommandHandler
    : IRequestHandler<VincularParticipanteExistenteCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ICredencialAlternaRepository _credencialesAlternas;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public VincularParticipanteExistenteCommandHandler(
        IUsuarioRepository usuarios,
        ICredencialAlternaRepository credencialesAlternas,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _usuarios = usuarios;
        _credencialesAlternas = credencialesAlternas;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(VincularParticipanteExistenteCommand request, CancellationToken cancellationToken)
    {
        if (request.UsuarioObjetivoId == request.UsuarioAVincularId)
            throw new BusinessException("Un participante no puede vincularse consigo mismo.");

        var objetivo = await _usuarios.ObtenerPorIdAsync(request.UsuarioObjetivoId)
            ?? throw new EntityNotFoundException("Usuario objetivo");

        var aVincular = await _usuarios.ObtenerPorIdAsync(request.UsuarioAVincularId)
            ?? throw new EntityNotFoundException("Participante a vincular");

        // Si ya está vinculado, solo se bloquea si es a OTRA cuenta
        // distinta -- volver a vincularlo hacia el MISMO objetivo debe
        // permitirse siempre, ya que es la forma de "refrescar" el
        // correo/contraseña copiados (ej. después de editarle el
        // correo, o cualquier otro motivo).
        if (aVincular.EsCuentaVinculada)
        {
            var vinculacionActual = await _credencialesAlternas.ObtenerPorCorreoAsync(aVincular.Correo.Value, cancellationToken);
            if (vinculacionActual != null && vinculacionActual.UsuarioId != request.UsuarioObjetivoId)
                throw new BusinessException("Ese participante ya está vinculado a otra cuenta.");
        }

        var quienEdita = _currentUser.UserId;

        // Se copian el correo+contraseña YA EXISTENTES del participante
        // a vincular -- nada nuevo que capturar, entra con lo mismo de
        // siempre, solo que ahora lo lleva a la cuenta objetivo.
        var existente = await _credencialesAlternas.ObtenerPorUsuarioIdAsync(request.UsuarioObjetivoId, cancellationToken);

        if (existente != null)
        {
            existente.Actualizar(aVincular.Correo, aVincular.PasswordHash, quienEdita);
        }
        else
        {
            var nueva = CredencialAlterna.Crear(
                request.UsuarioObjetivoId,
                aVincular.Correo,
                aVincular.PasswordHash,
                quienEdita);

            await _credencialesAlternas.AgregarAsync(nueva, cancellationToken);
        }

        aVincular.MarcarComoVinculada(true, quienEdita);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
