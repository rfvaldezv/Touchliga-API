using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Pago.Registrar;
using Touchliga.Application.Common.Interfaces;
using DomainPago = Touchliga.Domain.Entities.Pago;

namespace Touchliga.Application.Handlers.Pago.Registrar;

public sealed class RegistrarPagoCommandHandler : IRequestHandler<RegistrarPagoCommand, long>
{
    private readonly IPagoRepository _pagos;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public RegistrarPagoCommandHandler(
        IPagoRepository pagos,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _pagos = pagos;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(RegistrarPagoCommand request, CancellationToken cancellationToken)
    {
        // No se bloquea si ya existe un pago previo — puede ser el
        // segundo pago (la otra mitad de la cuota, o un ajuste que
        // haga el admin a mano).
        var pago = DomainPago.Registrar(
            request.UsuarioId,
            request.TemporadaId,
            request.Monto,
            request.MetodoPago,
            request.FechaPago,
            request.Referencia,
            _currentUser.UserId);

        await _pagos.AgregarAsync(pago, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return pago.Id;
    }
}
