using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

using Touchliga.Application.Common.Interfaces;
using Touchliga.Domain.Interfaces;

namespace Touchliga.Infrastructure.Email;

public sealed class SmtpEmailService : IEmailService
{
    private readonly EmailOptions _opcionesRespaldo;
    private readonly IConfiguracionSmtpRepository _configuraciones;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(
        IOptions<EmailOptions> opcionesRespaldo,
        IConfiguracionSmtpRepository configuraciones,
        ILogger<SmtpEmailService> logger)
    {
        _opcionesRespaldo = opcionesRespaldo.Value;
        _configuraciones = configuraciones;
        _logger = logger;
    }

    public async Task EnviarAsync(
        string destinatario,
        string asunto,
        string cuerpoHtml,
        CancellationToken cancellationToken = default)
    {
        // La configuración editable desde Administración (guardada en
        // base de datos) siempre tiene prioridad -- appsettings.json
        // solo sirve de respaldo mientras nadie ha guardado nada ahí.
        var config = await _configuraciones.ObtenerAsync(cancellationToken);

        var habilitado = config?.Habilitado ?? _opcionesRespaldo.Habilitado;
        var host = config?.Host ?? _opcionesRespaldo.Host;
        var port = config?.Port ?? _opcionesRespaldo.Port;
        var username = config?.Username ?? _opcionesRespaldo.Username;
        var password = config?.Password ?? _opcionesRespaldo.Password;
        var fromEmail = config?.FromEmail ?? _opcionesRespaldo.FromEmail;
        var fromName = config?.FromName ?? _opcionesRespaldo.FromName;

        if (!habilitado)
        {
            _logger.LogInformation(
                "Correo NO enviado (SMTP deshabilitado/sin configurar). Destinatario: {Destinatario}, Asunto: {Asunto}",
                destinatario, asunto);
            return;
        }

        var mensaje = new MimeMessage();
        mensaje.From.Add(new MailboxAddress(fromName, fromEmail));
        mensaje.To.Add(MailboxAddress.Parse(destinatario));
        mensaje.Subject = asunto;
        mensaje.Body = new BodyBuilder { HtmlBody = cuerpoHtml }.ToMessageBody();

        // Tiempo límite propio (10s) además del que traiga el token de
        // la petición -- así, si el servidor SMTP tarda o no responde,
        // el intento falla rápido en vez de colgarse indefinidamente
        // (crítico en hosting compartido, donde una tarea que tarda
        // demasiado puede terminar cortada a la mitad sin avisar).
        using var limiteTiempo = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        limiteTiempo.CancelAfter(TimeSpan.FromSeconds(10));

        try
        {
            using var cliente = new SmtpClient();

            await cliente.ConnectAsync(host, port, SecureSocketOptions.StartTls, limiteTiempo.Token);
            await cliente.AuthenticateAsync(username, password, limiteTiempo.Token);
            await cliente.SendAsync(mensaje, limiteTiempo.Token);
            await cliente.DisconnectAsync(true, limiteTiempo.Token);

            _logger.LogInformation(
                "Correo enviado correctamente a {Destinatario}. Asunto: {Asunto}",
                destinatario, asunto);
        }
        catch (OperationCanceledException) when (limiteTiempo.IsCancellationRequested)
        {
            _logger.LogError(
                "No se pudo enviar el correo a {Destinatario} -- el servidor SMTP no respondió en 10 segundos.",
                destinatario);
        }
        catch (Exception ex)
        {
            // Un correo que falla nunca debe tronar la acción principal
            // del usuario (guardar sus pronósticos) — solo se registra.
            _logger.LogError(ex, "No se pudo enviar el correo a {Destinatario}", destinatario);
        }
    }
}
