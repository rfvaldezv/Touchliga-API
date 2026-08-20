using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

/// <summary>
/// Configuración del servidor de correo (SMTP) usado para enviar
/// avisos a los participantes -- una sola fila, editable desde
/// Administración, para poder cambiar de proveedor o credenciales
/// sin necesitar republicar el API.
/// </summary>
public sealed class ConfiguracionSmtp : AggregateRoot
{
    private ConfiguracionSmtp()
    {
    }

    public bool Habilitado { get; private set; }
    public string Host { get; private set; } = string.Empty;
    public int Port { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string FromEmail { get; private set; } = string.Empty;
    public string FromName { get; private set; } = string.Empty;

    public static ConfiguracionSmtp Crear(
        bool habilitado,
        string host,
        int port,
        string username,
        string password,
        string fromEmail,
        string fromName,
        long usuarioId)
    {
        Validar(host, port, fromEmail, fromName, habilitado);

        return new ConfiguracionSmtp
        {
            Habilitado = habilitado,
            Host = host.Trim(),
            Port = port,
            Username = username.Trim(),
            Password = password,
            FromEmail = fromEmail.Trim(),
            FromName = fromName.Trim(),
            UsuarioAltaId = usuarioId,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }

    public void Editar(
        bool habilitado,
        string host,
        int port,
        string username,
        string password,
        string fromEmail,
        string fromName,
        long usuarioId)
    {
        Validar(host, port, fromEmail, fromName, habilitado);

        Habilitado = habilitado;
        Host = host.Trim();
        Port = port;
        Username = username.Trim();
        Password = password;
        FromEmail = fromEmail.Trim();
        FromName = fromName.Trim();

        MarcarModificado(usuarioId);
    }

    private static void Validar(string host, int port, string fromEmail, string fromName, bool habilitado)
    {
        if (!habilitado) return; // Si está deshabilitado, no exigimos datos completos.

        if (string.IsNullOrWhiteSpace(host))
            throw new DomainException("El servidor (host) es obligatorio.");

        if (port <= 0 || port > 65535)
            throw new DomainException("El puerto no es válido.");

        if (string.IsNullOrWhiteSpace(fromEmail))
            throw new DomainException("El correo remitente es obligatorio.");

        if (string.IsNullOrWhiteSpace(fromName))
            throw new DomainException("El nombre remitente es obligatorio.");
    }
}
