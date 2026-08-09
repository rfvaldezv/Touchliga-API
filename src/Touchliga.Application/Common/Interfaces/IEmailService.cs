namespace Touchliga.Application.Common.Interfaces;

/// <summary>
/// Envío de correo genérico — pensado para reutilizarse en
/// cualquier disparador futuro (confirmación de pronósticos,
/// recordatorio de pago, aviso de jornada por cerrar, etc.), no
/// solo el trigger actual.
/// </summary>
public interface IEmailService
{
    Task EnviarAsync(
        string destinatario,
        string asunto,
        string cuerpoHtml,
        CancellationToken cancellationToken = default);
}
