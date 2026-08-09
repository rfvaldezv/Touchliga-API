using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

/// <summary>
/// Archivo binario genérico (foto de perfil, escudo, banner de
/// patrocinador, etc.) guardado directamente en la base de datos —
/// no se depende de ningún servicio externo de almacenamiento.
/// Se sirve de vuelta vía GET /api/archivos/{id}, así que el mismo
/// campo "Url" que ya usan Equipo/Patrocinador puede apuntar aquí
/// sin cambiar esos modelos.
/// </summary>
public sealed class Archivo : AggregateRoot
{
    private Archivo()
    {
    }

    public string NombreArchivo { get; private set; } = string.Empty;

    public string ContentType { get; private set; } = string.Empty;

    public byte[] Datos { get; private set; } = [];

    public long UsuarioSubioId { get; private set; }

    public static Archivo Subir(
        string nombreArchivo,
        string contentType,
        byte[] datos,
        long usuarioSubioId)
    {
        if (datos.Length == 0)
            throw new DomainException("El archivo está vacío.");

        // 5 MB — suficiente para fotos de celular comprimidas, evita
        // que alguien suba algo enorme por accidente.
        const int maxBytes = 5 * 1024 * 1024;

        if (datos.Length > maxBytes)
            throw new DomainException("El archivo no puede pesar más de 5 MB.");

        return new Archivo
        {
            NombreArchivo = nombreArchivo,
            ContentType = contentType,
            Datos = datos,
            UsuarioSubioId = usuarioSubioId,
            UsuarioAltaId = usuarioSubioId,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }
}
