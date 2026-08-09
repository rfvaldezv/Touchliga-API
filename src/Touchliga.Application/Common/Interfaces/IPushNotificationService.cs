namespace Touchliga.Application.Common.Interfaces;

/// <summary>
/// Envío de notificaciones push — pensado para reutilizarse en
/// cualquier disparador futuro (jornada por cerrar, resultado
/// capturado, nuevo mensaje, anuncio publicado, etc.).
/// </summary>
public interface IPushNotificationService
{
    Task EnviarAUsuarioAsync(
        long usuarioId,
        string titulo,
        string cuerpo,
        CancellationToken cancellationToken = default);

    /// <summary>A todos los participantes con un dispositivo registrado.</summary>
    Task EnviarATodosAsync(
        string titulo,
        string cuerpo,
        CancellationToken cancellationToken = default);
}
