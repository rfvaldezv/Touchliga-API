using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Premio.Decidir;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.PremioOtorgado;

namespace Touchliga.Application.Handlers.Premio.Decidir;

public sealed class DecidirPremioCommandHandler : IRequestHandler<DecidirPremioCommand, Unit>
{
    private readonly IPremioOtorgadoRepository _decisiones;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DecidirPremioCommandHandler(
        IPremioOtorgadoRepository decisiones,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _decisiones = decisiones;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DecidirPremioCommand request, CancellationToken cancellationToken)
    {
        var existente = await _decisiones.ObtenerAsync(
            request.Ambito, request.ReferenciaId, request.UsuarioId, cancellationToken);

        if (existente != null)
        {
            existente.Redecidir(request.Estado, request.MontoAjustado, request.Motivo, _currentUser.UserId);
            _decisiones.Actualizar(existente);
        }
        else
        {
            var nueva = DomainEntity.Decidir(
                request.Ambito,
                request.ReferenciaId,
                request.UsuarioId,
                request.Estado,
                request.MontoAjustado,
                request.Motivo,
                _currentUser.UserId);

            await _decisiones.AgregarAsync(nueva, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
