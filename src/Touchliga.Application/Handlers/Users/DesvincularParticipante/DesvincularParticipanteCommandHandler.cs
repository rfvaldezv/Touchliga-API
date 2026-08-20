using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Users.Commands.DesvincularParticipante;

namespace Touchliga.Application.Handlers.Users.DesvincularParticipante;

public sealed class DesvincularParticipanteCommandHandler : IRequestHandler<DesvincularParticipanteCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ICredencialAlternaRepository _credencialesAlternas;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DesvincularParticipanteCommandHandler(
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

    public async Task<Unit> Handle(DesvincularParticipanteCommand request, CancellationToken cancellationToken)
    {
        var vinculado = await _usuarios.ObtenerPorIdAsync(request.UsuarioVinculadoId)
            ?? throw new EntityNotFoundException("Participante vinculado");

        var credencial = await _credencialesAlternas.ObtenerPorUsuarioIdAsync(request.UsuarioObjetivoId, cancellationToken);

        if (credencial != null)
        {
            _credencialesAlternas.Eliminar(credencial);
        }

        vinculado.MarcarComoVinculada(false, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
