using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Infrastructure.Pagos;

public sealed class StripePagoService : IPagoStripeService
{
    private readonly StripeOptions _options;
    private readonly ILogger<StripePagoService> _logger;

    public StripePagoService(IOptions<StripeOptions> options, ILogger<StripePagoService> logger)
    {
        _options = options.Value;
        _logger = logger;

        if (_options.Habilitado && !string.IsNullOrWhiteSpace(_options.SecretKey))
            StripeConfiguration.ApiKey = _options.SecretKey;
    }

    public async Task<string> CrearSesionCheckoutAsync(
        long usuarioId,
        long temporadaId,
        decimal monto,
        string nombreTemporada,
        string successUrl,
        string cancelUrl,
        CancellationToken cancellationToken = default)
    {
        if (!_options.Habilitado)
            throw new InvalidOperationException("Los pagos con tarjeta todavía no están configurados.");

        // Stripe recibe el monto en la unidad mínima de la moneda
        // (centavos para MXN/USD) — de ahí el * 100.
        var montoEnCentavos = (long)Math.Round(monto * 100, MidpointRounding.AwayFromZero);

        var opciones = new SessionCreateOptions
        {
            Mode = "payment",
            PaymentMethodTypes = ["card"],
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = _options.Moneda,
                        UnitAmount = montoEnCentavos,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Cuota Touchliga — {nombreTemporada}",
                        },
                    },
                },
            ],
            // Aquí va codificado a quién y a qué temporada
            // corresponde este pago — así el webhook, cuando Stripe
            // confirme el cobro, sabe exactamente qué registrar sin
            // tener que guardar una sesión "pendiente" en la BD.
            ClientReferenceId = $"{usuarioId}:{temporadaId}",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
        };

        var service = new SessionService();
        var sesion = await service.CreateAsync(opciones, cancellationToken: cancellationToken);

        return sesion.Url;
    }

    public ResultadoWebhookStripe ProcesarWebhook(string payloadJson, string firmaStripe)
    {
        Event evento;

        try
        {
            // throwOnApiVersionMismatch: false — la cuenta de Stripe
            // puede estar en una versión de API más nueva que la que
            // trae la librería instalada; sin esto, Stripe.net rechaza
            // el evento entero con una excepción en vez de solo
            // avisar. Es la forma oficial recomendada por Stripe de
            // manejarlo cuando no dependemos de campos específicos
            // que hayan cambiado entre versiones.
            evento = EventUtility.ConstructEvent(
                payloadJson, firmaStripe, _options.WebhookSecret, throwOnApiVersionMismatch: false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo procesar el webhook de Stripe (firma inválida u otro motivo) — se ignora.");
            return new ResultadoWebhookStripe(false, null, null, 0, string.Empty);
        }

        if (evento.Type != "checkout.session.completed")
            return new ResultadoWebhookStripe(false, null, null, 0, string.Empty);

        var sesion = evento.Data.Object as Session;

        if (sesion is null || string.IsNullOrEmpty(sesion.ClientReferenceId))
            return new ResultadoWebhookStripe(false, null, null, 0, string.Empty);

        var partes = sesion.ClientReferenceId.Split(':');

        if (partes.Length != 2 ||
            !long.TryParse(partes[0], out var usuarioId) ||
            !long.TryParse(partes[1], out var temporadaId))
        {
            _logger.LogWarning(
                "ClientReferenceId de Stripe con formato inesperado: {Valor}", sesion.ClientReferenceId);
            return new ResultadoWebhookStripe(false, null, null, 0, string.Empty);
        }

        var monto = (sesion.AmountTotal ?? 0) / 100m;

        return new ResultadoWebhookStripe(true, usuarioId, temporadaId, monto, sesion.Id);
    }
}
