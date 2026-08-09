using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.Commands.ActualizarPerfil;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Users.ActualizarPerfil;

public sealed class ActualizarPerfilCommandHandler : IRequestHandler<ActualizarPerfilCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public ActualizarPerfilCommandHandler(
        IUsuarioRepository usuarios,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _usuarios = usuarios;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(ActualizarPerfilCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(_currentUser.UserId)
            ?? throw new EntityNotFoundException("Usuario");

        usuario.ActualizarPerfilExtendido(
            request.FechaNacimiento,
            request.EquipoFavoritoId,
            request.Nickname,
            request.FotoUrl,
            _currentUser.UserId);

        await _usuarios.ActualizarAsync(usuario);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
