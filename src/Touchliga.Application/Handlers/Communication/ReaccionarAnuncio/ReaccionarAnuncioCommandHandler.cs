using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Communication.Commands.ReaccionarAnuncio;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.ReaccionAnuncio;

namespace Touchliga.Application.Handlers.Communication.ReaccionarAnuncio;

public sealed class ReaccionarAnuncioCommandHandler : IRequestHandler<ReaccionarAnuncioCommand, Unit>
{
    private readonly IReaccionAnuncioRepository _reacciones;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public ReaccionarAnuncioCommandHandler(
        IReaccionAnuncioRepository reacciones,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _reacciones = reacciones;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(ReaccionarAnuncioCommand request, CancellationToken cancellationToken)
    {
        var existente = await _reacciones.ObtenerAsync(
            request.AnuncioId, _currentUser.UserId, cancellationToken);

        if (existente == null)
        {
            var nueva = DomainEntity.Crear(request.AnuncioId, _currentUser.UserId, request.Emoji);
            await _reacciones.AgregarAsync(nueva, cancellationToken);
        }
        else if (existente.Emoji == request.Emoji)
        {
            // Mismo emoji que ya tenías -> se quita (alternar).
            _reacciones.Eliminar(existente);
        }
        else
        {
            existente.Cambiar(request.Emoji);
            _reacciones.Actualizar(existente);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
