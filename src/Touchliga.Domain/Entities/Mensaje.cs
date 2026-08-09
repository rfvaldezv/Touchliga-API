using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

/// <summary>
/// Mensaje directo entre dos usuarios (incluye el caso de un
/// participante escribiéndole a un administrador/capturador —
/// no hay distinción especial de "canal", es el mismo modelo).
/// </summary>
public sealed class Mensaje : AggregateRoot
{
    private Mensaje()
    {
    }

    public long RemitenteId { get; private set; }

    public long DestinatarioId { get; private set; }

    public string Contenido { get; private set; } = string.Empty;

    /// <summary>Opcional — URL de una imagen ya subida vía /api/archivos.
    /// Un mensaje puede ser solo imagen, sin texto.</summary>
    public string? ImagenUrl { get; private set; }

    public DateTime FechaEnvio { get; private set; }

    public bool Leido { get; private set; }

    public static Mensaje Crear(
        long remitenteId,
        long destinatarioId,
        string contenido,
        string? imagenUrl = null)
    {
        var tieneImagen = !string.IsNullOrWhiteSpace(imagenUrl);

        if (string.IsNullOrWhiteSpace(contenido) && !tieneImagen)
            throw new DomainException("El mensaje no puede estar vacío.");

        if (remitenteId == destinatarioId)
            throw new DomainException("No puedes enviarte un mensaje a ti mismo.");

        return new Mensaje
        {
            RemitenteId = remitenteId,
            DestinatarioId = destinatarioId,
            Contenido = contenido?.Trim() ?? string.Empty,
            ImagenUrl = tieneImagen ? imagenUrl!.Trim() : null,
            FechaEnvio = DateTime.UtcNow,
            Leido = false,
            UsuarioAltaId = remitenteId,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }

    public void MarcarLeido()
    {
        Leido = true;
    }

    /// <summary>Solo lo puede llamar el remitente — se valida en el handler.</summary>
    public void Editar(string contenido, long usuarioId, string? imagenUrl = null)
    {
        var tieneImagen = !string.IsNullOrWhiteSpace(imagenUrl);

        if (string.IsNullOrWhiteSpace(contenido) && !tieneImagen)
            throw new DomainException("El mensaje no puede estar vacío.");

        Contenido = contenido?.Trim() ?? string.Empty;
        ImagenUrl = tieneImagen ? imagenUrl!.Trim() : null;

        MarcarModificado(usuarioId);
    }
}
