using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

/// <summary>
/// Anuncio del administrador/capturador hacia todos los participantes.
/// Solo lectura para quien no lo publicó.
/// </summary>
public sealed class Anuncio : AggregateRoot
{
    private Anuncio()
    {
    }

    public string Titulo { get; private set; } = string.Empty;

    public string Contenido { get; private set; } = string.Empty;

    /// <summary>Opcional — URL de una imagen ya subida vía /api/archivos.</summary>
    public string? ImagenUrl { get; private set; }

    public long UsuarioAutorId { get; private set; }

    public DateTime FechaPublicacion { get; private set; }

    public static Anuncio Crear(
        string titulo,
        string contenido,
        long usuarioAutorId,
        string? imagenUrl = null)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("El título es obligatorio.");

        if (string.IsNullOrWhiteSpace(contenido))
            throw new DomainException("El contenido es obligatorio.");

        return new Anuncio
        {
            Titulo = titulo.Trim(),
            Contenido = contenido.Trim(),
            ImagenUrl = string.IsNullOrWhiteSpace(imagenUrl) ? null : imagenUrl.Trim(),
            UsuarioAutorId = usuarioAutorId,
            FechaPublicacion = DateTime.UtcNow,
            UsuarioAltaId = usuarioAutorId,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }

    /// <summary>
    /// Corrige un anuncio ya publicado. Se manda de nuevo el push a
    /// todos, por eso el llamador decide si reenviar o no según lo
    /// que cambió — esta capa solo actualiza el texto.
    /// </summary>
    public void Editar(string titulo, string contenido, long usuarioId, string? imagenUrl = null)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("El título es obligatorio.");

        if (string.IsNullOrWhiteSpace(contenido))
            throw new DomainException("El contenido es obligatorio.");

        Titulo = titulo.Trim();
        Contenido = contenido.Trim();
        ImagenUrl = string.IsNullOrWhiteSpace(imagenUrl) ? null : imagenUrl.Trim();

        MarcarModificado(usuarioId);
    }
}
