namespace Touchliga.Infrastructure.Pagos;

public sealed class StripeOptions
{
    public const string SectionName = "Stripe";

    public bool Habilitado { get; set; }
    public string SecretKey { get; set; } = string.Empty;
    public string WebhookSecret { get; set; } = string.Empty;
    public string Moneda { get; set; } = "mxn";
}
