using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Touchliga.Domain.Interfaces;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Infrastructure.Push;

public sealed class FirebasePushNotificationService : IPushNotificationService
{
    private readonly PushOptions _options;
    private readonly IPushTokenRepository _tokens;
    private readonly ILogger<FirebasePushNotificationService> _logger;

    private static FirebaseApp? _app;
    private static readonly object _lock = new();

    public FirebasePushNotificationService(
        IOptions<PushOptions> options,
        IPushTokenRepository tokens,
        ILogger<FirebasePushNotificationService> logger)
    {
        _options = options.Value;
        _tokens = tokens;
        _logger = logger;
    }

    private FirebaseApp? ObtenerApp()
    {
        if (!_options.Habilitado || string.IsNullOrWhiteSpace(_options.CredencialesJson))
            return null;

        if (_app != null) return _app;

        lock (_lock)
        {
            _app ??= FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(_options.CredencialesJson),
            });
        }

        return _app;
    }

    public async Task EnviarAUsuarioAsync(
        long usuarioId,
        string titulo,
        string cuerpo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tokens = await _tokens.ObtenerPorUsuarioAsync(usuarioId, cancellationToken);
            await EnviarATokensAsync(tokens.Select(t => t.Token).ToList(), titulo, cuerpo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "No se pudo enviar el push al usuario {UsuarioId}. Título: {Titulo}", usuarioId, titulo);
        }
    }

    public async Task EnviarATodosAsync(
        string titulo,
        string cuerpo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tokens = await _tokens.ObtenerTodosAsync(cancellationToken);
            await EnviarATokensAsync(tokens.Select(t => t.Token).ToList(), titulo, cuerpo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "No se pudo enviar el push a todos. Título: {Titulo}", titulo);
        }
    }

    private async Task EnviarATokensAsync(IReadOnlyList<string> tokens, string titulo, string cuerpo)
    {
        if (tokens.Count == 0) return;

        try
        {
            var app = ObtenerApp();

            if (app is null)
            {
                _logger.LogInformation(
                    "Push NO enviado (deshabilitado/sin configurar) a {Cantidad} dispositivo(s). Título: {Titulo}",
                    tokens.Count, titulo);
                return;
            }

            var mensaje = new MulticastMessage
            {
                Tokens = tokens,
                Notification = new Notification { Title = titulo, Body = cuerpo },
            };

            var respuesta = await FirebaseMessaging.GetMessaging(app).SendEachForMulticastAsync(mensaje);

            _logger.LogInformation(
                "Push enviado: {Exitosos} de {Total} dispositivos. Título: {Titulo}",
                respuesta.SuccessCount, tokens.Count, titulo);
        }
        catch (Exception ex)
        {
            // Un push que falla (o que ni siquiera logra inicializar
            // Firebase) nunca debe tumbar la acción principal del
            // usuario (publicar un anuncio, mandar un mensaje) — solo
            // se registra.
            _logger.LogError(ex, "No se pudo enviar el push. Título: {Titulo}", titulo);
        }
    }
}
