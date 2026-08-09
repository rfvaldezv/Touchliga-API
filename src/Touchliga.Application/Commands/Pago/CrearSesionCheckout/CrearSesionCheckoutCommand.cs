using MediatR;

namespace Touchliga.Application.Commands.Pago.CrearSesionCheckout;

/// <summary>tipoPago: "Completo" o "Mitad".</summary>
public sealed record CrearSesionCheckoutCommand(long TemporadaId, string TipoPago) : IRequest<string>;
