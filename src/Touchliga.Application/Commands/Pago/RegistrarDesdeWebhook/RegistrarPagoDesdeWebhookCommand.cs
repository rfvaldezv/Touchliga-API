using MediatR;

namespace Touchliga.Application.Commands.Pago.RegistrarDesdeWebhook;

public sealed record RegistrarPagoDesdeWebhookCommand(
    long UsuarioId,
    long TemporadaId,
    decimal Monto,
    string StripeSessionId
) : IRequest<Unit>;
