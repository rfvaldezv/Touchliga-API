using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.Users.DTOs;
using Touchliga.Application.Users.Commands.CrearUsuario;
using Touchliga.Application.Users.Commands.AsignarRol;
using Touchliga.Application.Users.Commands.QuitarRol;
using Touchliga.Application.Users.Commands.EditarInfoUsuario;
using Touchliga.Application.Users.Commands.RestablecerPassword;
using Touchliga.Application.Users.Commands.CambiarMiPassword;
using Touchliga.Application.Users.Commands.CambiarEstatusUsuario;
using Touchliga.Application.Users.Queries.GetUsuarios;
using Touchliga.Application.Users.Queries.GetRoles;

namespace Touchliga.Api.Controllers;

/// <summary>
/// Gestión de usuarios y roles. Solo Administrador.
/// </summary>
[ApiController]
[Authorize(Roles = "Administrador")]
[Route("api/usuarios")]
public sealed class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UsuarioAdminDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetUsuariosQuery()));
    }

    [HttpGet("roles")]
    public async Task<ActionResult<IReadOnlyList<RolDto>>> GetRoles()
    {
        return Ok(await _mediator.Send(new GetRolesQuery()));
    }

    [HttpPost]
    public async Task<ActionResult<long>> Crear([FromBody] CrearUsuarioCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("{usuarioId:long}/roles/{rolId:long}")]
    public async Task<IActionResult> AsignarRol(long usuarioId, long rolId)
    {
        await _mediator.Send(new AsignarRolCommand(usuarioId, rolId));
        return NoContent();
    }

    [HttpDelete("{usuarioId:long}/roles/{rolId:long}")]
    public async Task<IActionResult> QuitarRol(long usuarioId, long rolId)
    {
        await _mediator.Send(new QuitarRolCommand(usuarioId, rolId));
        return NoContent();
    }

    [HttpPut("{usuarioId:long}/info")]
    public async Task<IActionResult> EditarInfo(long usuarioId, [FromBody] EditarInfoUsuarioRequest body)
    {
        await _mediator.Send(new EditarInfoUsuarioCommand(
            usuarioId, body.Nombre, body.Apellidos, body.Telefono, body.Correo, body.CiudadId, body.PaisId, body.EstadoId));
        return NoContent();
    }

    /// <summary>Soporte: genera una contraseña temporal nueva y la
    /// regresa en texto plano UNA sola vez para que el admin se la
    /// comparta al participante.</summary>
    [HttpPost("{usuarioId:long}/restablecer-password")]
    public async Task<ActionResult<string>> RestablecerPassword(long usuarioId)
    {
        var nuevaPassword = await _mediator.Send(new RestablecerPasswordCommand(usuarioId));
        return Ok(new { password = nuevaPassword });
    }

    /// <summary>Cualquier participante autenticado cambia su propia
    /// contraseña desde Perfil — no requiere rol de Administrador,
    /// por eso se sobreescribe la autorización de la clase.</summary>
    [Authorize]
    [HttpPost("mi-password")]
    public async Task<IActionResult> CambiarMiPassword([FromBody] CambiarMiPasswordRequest body)
    {
        await _mediator.Send(new CambiarMiPasswordCommand(body.PasswordActual, body.PasswordNueva));
        return NoContent();
    }

    [HttpPut("{usuarioId:long}/estatus")]
    public async Task<IActionResult> CambiarEstatus(long usuarioId, [FromBody] CambiarEstatusUsuarioCommand command)
    {
        await _mediator.Send(command with { UsuarioId = usuarioId });
        return NoContent();
    }
}

public sealed record EditarInfoUsuarioRequest(
    string Nombre,
    string Apellidos,
    string Telefono,
    string Correo,
    long? CiudadId,
    long? PaisId,
    long? EstadoId
);

public sealed record CambiarMiPasswordRequest(string PasswordActual, string PasswordNueva);
