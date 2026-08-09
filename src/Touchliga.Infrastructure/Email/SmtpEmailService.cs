using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Infrastructure.Email;

public sealed class SmtpEmailService : IEmailService
{
    private readonly EmailOptions _options;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptions<EmailOptions> options, ILogger<SmtpEmailService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task EnviarAsync(
        string destinatario,
        string asunto,
        string cuerpoHtml,
        CancellationToken cancellationToken = default)
    {
        if (!_options.Habilitado)
        {
            _logger.LogInformation(
                "Correo NO enviado (SMTP deshabilitado/sin configurar). Destinatario: {Destinatario}, Asunto: {Asunto}",
                destinatario, asunto);
            return;
        }

        var mensaje = new MimeMessage();
        mensaje.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
        mensaje.To.Add(MailboxAddress.Parse(destinatario));
        mensaje.Subject = asunto;
        mensaje.Body = new BodyBuilder { HtmlBody = cuerpoHtml }.ToMessageBody();

        try
        {
            using var cliente = new SmtpClient();

            await cliente.ConnectAsync(
                _options.Host,
                _options.Port,
                SecureSocketOptions.StartTls,
                cancellationToken);

            await cliente.AuthenticateAsync(_options.Username, _options.Password, cancellationToken);
            await cliente.SendAsync(mensaje, cancellationToken);
            await cliente.DisconnectAsync(true, cancellationToken);

            _logger.LogInformation(
                "Correo enviado correctamente a {Destinatario}. Asunto: {Asunto}",
                destinatario, asunto);
        }
        catch (Exception ex)
        {
            // Un correo que falla nunca debe tronar la acción principal
            // del usuario (guardar sus pronósticos) — solo se registra.
            _logger.LogError(ex, "No se pudo enviar el correo a {Destinatario}", destinatario);
        }
    }
}
