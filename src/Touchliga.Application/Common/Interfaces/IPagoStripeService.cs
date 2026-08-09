namespace Touchliga.Application.Common.Interfaces;

public sealed record ResultadoWebhookStripe(
    bool EsPagoCompletado,
    long? UsuarioId,
    long? TemporadaId,
    decimal Monto,
    string SessionId);

public interface IPagoStripeService
{
    /// <summary>
    /// Crea una sesión de Stripe Checkout para que el participante
    /// pague la cuota de esa temporada, y regresa la URL a la que
    /// hay que mandarlo (página alojada por Stripe, sin necesidad
    /// de ningún SDK nativo del lado de la app).
    /// </summary>
    Task<string> CrearSesionCheckoutAsync(
        long usuarioId,
        long temporadaId,
        decimal monto,
        string nombreTemporada,
        string successUrl,
        string cancelUrl,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida la firma del webhook (para confirmar que la petición
    /// de verdad viene de Stripe, no de cualquiera) y extrae los
    /// datos del pago si el evento es de un checkout completado.
    /// </summary>
    ResultadoWebhookStripe ProcesarWebhook(string payloadJson, string firmaStripe);
}
