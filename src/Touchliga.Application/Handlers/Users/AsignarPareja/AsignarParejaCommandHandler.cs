using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Users.Commands.AsignarPareja;

namespace Touchliga.Application.Handlers.Users.AsignarPareja;

public sealed class AsignarParejaCommandHandler : IRequestHandler<AsignarParejaCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public AsignarParejaCommandHandler(
        IUsuarioRepository usuarios,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _usuarios = usuarios;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(AsignarParejaCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId)
            ?? throw new EntityNotFoundException("Usuario");

        var quienEdita = _currentUser.UserId;

        // Si ya tenía una pareja distinta a la nueva (o se está
        // desvinculando), esa pareja anterior también se desvincula
        // por completo -- así nunca queda un vínculo "a medias" donde
        // uno ve al otro como pareja pero no al revés.
        if (usuario.ParejaId.HasValue && usuario.ParejaId != request.ParejaId)
        {
            var parejaAnterior = await _usuarios.ObtenerPorIdAsync(usuario.ParejaId.Value);
            if (parejaAnterior != null)
            {
                parejaAnterior.AsignarPareja(null, null, quienEdita);
            }
        }

        usuario.AsignarPareja(request.ParejaId, request.NombreEquipo, quienEdita);

        // Si se está vinculando con alguien nuevo, ese alguien también
        // queda vinculado de vuelta, con el mismo apodo de equipo.
        if (request.ParejaId.HasValue)
        {
            var parejaNueva = await _usuarios.ObtenerPorIdAsync(request.ParejaId.Value)
                ?? throw new EntityNotFoundException("Participante pareja");

            parejaNueva.AsignarPareja(request.UsuarioId, request.NombreEquipo, quienEdita);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
