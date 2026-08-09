using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Api.Controllers;

/// <summary>
/// Subida y descarga de archivos binarios (fotos de perfil, escudos,
/// banners de patrocinador). Se guardan directamente en la base de
/// datos — no depende de ningún servicio externo de almacenamiento.
/// La URL que regresa /api/archivos ya se puede usar tal cual en
/// cualquier campo que hoy pide una "URL de imagen" (EscudoUrl,
/// ImagenUrl de Patrocinador, FotoUrl de Usuario, etc.).
/// </summary>
[ApiController]
[Route("api/archivos")]
public sealed class ArchivosController : ControllerBase
{
    private static readonly HashSet<string> TiposPermitidos = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/png", "image/webp", "image/gif"
    };

    private readonly IArchivoRepository _archivos;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public ArchivosController(
        IArchivoRepository archivos,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _archivos = archivos;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    [Authorize]
    [HttpPost]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> Subir(IFormFile archivo, CancellationToken cancellationToken)
    {
        if (archivo is null || archivo.Length == 0)
            return BadRequest(new { message = "No se recibió ningún archivo." });

        if (!TiposPermitidos.Contains(archivo.ContentType))
            return BadRequest(new { message = "Solo se permiten imágenes (jpg, png, webp, gif)." });

        using var stream = new MemoryStream();
        await archivo.CopyToAsync(stream, cancellationToken);

        var entidad = Archivo.Subir(
            archivo.FileName,
            archivo.ContentType,
            stream.ToArray(),
            _currentUser.UserId);

        await _archivos.AgregarAsync(entidad, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // URL absoluta usando el mismo host/puerto con el que llegó
        // la petición — así funciona igual en localhost, 10.0.2.2
        // (emulador) o la IP de la red local (celular físico).
        var url = $"{Request.Scheme}://{Request.Host}/api/archivos/{entidad.Id}";

        return Ok(new { id = entidad.Id, url });
    }

    /// <summary>
    /// Pública a propósito: las imágenes se muestran con
    /// Image.network en Flutter, que no manda el token de sesión.
    /// No hay nada sensible en una foto de perfil o un escudo.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> Descargar(long id, CancellationToken cancellationToken)
    {
        var archivo = await _archivos.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException("Archivo");

        return File(archivo.Datos, archivo.ContentType);
    }
}
