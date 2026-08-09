namespace Touchliga.Infrastructure.Email;

/// <summary>Se llena desde la sección "Smtp" de appsettings/user-secrets.</summary>
public sealed class EmailOptions
{
    public const string SectionName = "Smtp";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "Touchliga";

    /// <summary>
    /// Si es false, el servicio no manda nada de verdad — solo lo
    /// registra en el log. Útil mientras no haya credenciales SMTP
    /// configuradas, para que el resto de la app siga funcionando
    /// sin tronar.
    /// </summary>
    public bool Habilitado { get; set; }
}
