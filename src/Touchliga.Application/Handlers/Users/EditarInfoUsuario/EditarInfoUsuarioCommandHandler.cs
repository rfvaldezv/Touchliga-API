using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.Commands.EditarInfoUsuario;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Users.EditarInfoUsuario;

public sealed class EditarInfoUsuarioCommandHandler : IRequestHandler<EditarInfoUsuarioCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public EditarInfoUsuarioCommandHandler(
        IUsuarioRepository usuarios,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _usuarios = usuarios;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(EditarInfoUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId)
            ?? throw new EntityNotFoundException("Usuario");

        usuario.ActualizarInfoContacto(
            request.Nombre,
            request.Apellidos,
            request.Telefono,
            request.Correo,
            request.CiudadId,
            request.PaisId,
            request.EstadoId,
            _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
