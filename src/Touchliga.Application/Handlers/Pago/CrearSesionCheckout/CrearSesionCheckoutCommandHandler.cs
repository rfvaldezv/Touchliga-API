using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Pago.CrearSesionCheckout;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Pago.CrearSesionCheckout;

public sealed class CrearSesionCheckoutCommandHandler : IRequestHandler<CrearSesionCheckoutCommand, string>
{
    private readonly ITemporadaRepository _temporadas;
    private readonly IPagoRepository _pagos;
    private readonly IPagoStripeService _stripe;
    private readonly ICurrentUserService _currentUser;
    private readonly IAppUrlsService _appUrls;

    public CrearSesionCheckoutCommandHandler(
        ITemporadaRepository temporadas,
        IPagoRepository pagos,
        IPagoStripeService stripe,
        ICurrentUserService currentUser,
        IAppUrlsService appUrls)
    {
        _temporadas = temporadas;
        _pagos = pagos;
        _stripe = stripe;
        _currentUser = currentUser;
        _appUrls = appUrls;
    }

    public async Task<string> Handle(CrearSesionCheckoutCommand request, CancellationToken cancellationToken)
    {
        var temporada = await _temporadas.ObtenerPorIdAsync(request.TemporadaId, cancellationToken)
            ?? throw new EntityNotFoundException("Temporada");

        if (temporada.Cuota <= 0)
            throw new BusinessException("Esta temporada no tiene una cuota configurada.");

        var pagosExistentes = await _pagos.ObtenerListaPorUsuarioYTemporadaAsync(
            _currentUser.UserId, request.TemporadaId, cancellationToken);

        var totalPagado = pagosExistentes.Sum(p => p.Monto);
        var saldoPendiente = temporada.Cuota - totalPagado;

        if (saldoPendiente <= 0)
            throw new BusinessException("Ya cubriste por completo tu cuota de esta temporada.");

        var mitadDeCuota = Math.Round(temporada.Cuota / 2, 2);

        decimal monto = request.TipoPago switch
        {
            "Completo" => saldoPendiente,
            "Mitad" => mitadDeCuota <= saldoPendiente
                ? mitadDeCuota
                : throw new BusinessException(
                    "Ya casi terminas — paga el saldo restante completo en vez de otra mitad."),
            _ => throw new BusinessException("Tipo de pago inválido."),
        };

        var baseUrl = _appUrls.BaseUrlPublica;

        return await _stripe.CrearSesionCheckoutAsync(
            _currentUser.UserId,
            request.TemporadaId,
            monto,
            temporada.Nombre,
            successUrl: $"{baseUrl}/api/pagos/exitoso",
            cancelUrl: $"{baseUrl}/api/pagos/cancelado",
            cancellationToken);
    }
}
