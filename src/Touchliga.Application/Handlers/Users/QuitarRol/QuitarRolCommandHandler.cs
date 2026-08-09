using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.Commands.QuitarRol;

namespace Touchliga.Application.Handlers.Users.QuitarRol;

public sealed class QuitarRolCommandHandler : IRequestHandler<QuitarRolCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IUnitOfWork _unitOfWork;

    public QuitarRolCommandHandler(IUsuarioRepository usuarios, IUnitOfWork unitOfWork)
    {
        _usuarios = usuarios;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(QuitarRolCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId)
            ?? throw new EntityNotFoundException("Usuario");

        usuario.QuitarRol(request.RolId);

        await _usuarios.ActualizarAsync(usuario);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
