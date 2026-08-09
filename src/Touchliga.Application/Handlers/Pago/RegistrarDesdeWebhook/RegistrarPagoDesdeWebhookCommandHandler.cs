using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Pago.RegistrarDesdeWebhook;
using Touchliga.Application.Common.Interfaces;
using DomainPago = Touchliga.Domain.Entities.Pago;

namespace Touchliga.Application.Handlers.Pago.RegistrarDesdeWebhook;

/// <summary>
/// Registra el pago cuando Stripe confirma el cobro por webhook —
/// distinto del registro manual del admin (RegistrarPagoCommand),
/// aunque comparten la misma entidad de dominio. Es "a prueba de
/// duplicados": si Stripe reintenta el mismo webhook (les pasa
/// seguido), no se registra el pago dos veces.
/// </summary>
public sealed class RegistrarPagoDesdeWebhookCommandHandler
    : IRequestHandler<RegistrarPagoDesdeWebhookCommand, Unit>
{
    private readonly IPagoRepository _pagos;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarPagoDesdeWebhookCommandHandler(IPagoRepository pagos, IUnitOfWork unitOfWork)
    {
        _pagos = pagos;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RegistrarPagoDesdeWebhookCommand request, CancellationToken cancellationToken)
    {
        var yaRegistrado = await _pagos.ObtenerPorReferenciaAsync(request.StripeSessionId, cancellationToken);

        if (yaRegistrado != null)
            return Unit.Value;

        var pago = DomainPago.Registrar(
            request.UsuarioId,
            request.TemporadaId,
            request.Monto,
            "Stripe",
            DateTime.UtcNow,
            request.StripeSessionId,
            request.UsuarioId);

        await _pagos.AgregarAsync(pago, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
