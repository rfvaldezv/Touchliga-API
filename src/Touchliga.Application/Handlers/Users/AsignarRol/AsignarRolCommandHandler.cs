using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.Commands.AsignarRol;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Users.AsignarRol;

public sealed class AsignarRolCommandHandler : IRequestHandler<AsignarRolCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IRolRepository _roles;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public AsignarRolCommandHandler(
        IUsuarioRepository usuarios,
        IRolRepository roles,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _usuarios = usuarios;
        _roles = roles;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(AsignarRolCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId)
            ?? throw new EntityNotFoundException("Usuario");

        var rol = await _roles.ObtenerPorIdAsync(request.RolId)
            ?? throw new EntityNotFoundException("Rol");

        usuario.AsignarRol(rol, _currentUser.UserId);

        await _usuarios.ActualizarAsync(usuario);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
