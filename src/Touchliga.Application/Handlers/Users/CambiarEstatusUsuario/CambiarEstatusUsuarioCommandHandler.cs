using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.Commands.CambiarEstatusUsuario;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Users.CambiarEstatusUsuario;

public sealed class CambiarEstatusUsuarioCommandHandler : IRequestHandler<CambiarEstatusUsuarioCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CambiarEstatusUsuarioCommandHandler(
        IUsuarioRepository usuarios,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _usuarios = usuarios;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(CambiarEstatusUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId)
            ?? throw new EntityNotFoundException("Usuario");

        usuario.CambiarEstatus(request.Estatus, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
