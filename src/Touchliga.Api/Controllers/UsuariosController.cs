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
using Touchliga.Application.Users.Commands.AsignarPareja;
using Touchliga.Application.Users.Commands.AgregarCredencialAlterna;
using Touchliga.Application.Users.Commands.QuitarCredencialAlterna;
using Touchliga.Application.Users.Commands.VincularParticipanteExistente;
using Touchliga.Application.Users.Commands.DesvincularParticipante;
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

    /// <summary>Soporte: restablece la contraseña de un participante.
    /// Si el admin manda "nuevaPassword" en el body se usa esa; si no,
    /// se genera una aleatoria. Se regresa en texto plano UNA sola vez
    /// para que el admin se la comparta al participante.</summary>
    [HttpPost("{usuarioId:long}/restablecer-password")]
    public async Task<ActionResult<string>> RestablecerPassword(
        long usuarioId, [FromBody] RestablecerPasswordRequest? request)
    {
        var nuevaPassword = await _mediator.Send(
            new RestablecerPasswordCommand(usuarioId, request?.NuevaPassword));
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

    /// <summary>Vincula (o desvincula, mandando parejaId: null) a un
    /// participante con otro como pareja/equipo -- solo visual.</summary>
    [HttpPut("{usuarioId:long}/pareja")]
    public async Task<IActionResult> AsignarPareja(long usuarioId, [FromBody] AsignarParejaRequest body)
    {
        await _mediator.Send(new AsignarParejaCommand(usuarioId, body.ParejaId, body.NombreEquipo));
        return NoContent();
    }

    /// <summary>Registra (o reemplaza) un segundo correo+contraseña
    /// que puede iniciar sesión COMO este mismo participante -- mismos
    /// pronósticos, mismos puntos, mismo Id (pensado para parejas/
    /// familiares que juegan juntos).</summary>
    [HttpPut("{usuarioId:long}/credencial-alterna")]
    public async Task<IActionResult> AgregarCredencialAlterna(long usuarioId, [FromBody] AgregarCredencialAlternaRequest body)
    {
        await _mediator.Send(new AgregarCredencialAlternaCommand(usuarioId, body.Correo, body.Password));
        return NoContent();
    }

    [HttpDelete("{usuarioId:long}/credencial-alterna")]
    public async Task<IActionResult> QuitarCredencialAlterna(long usuarioId)
    {
        await _mediator.Send(new QuitarCredencialAlternaCommand(usuarioId));
        return NoContent();
    }

    /// <summary>Toma a un participante YA REGISTRADO y lo vincula
    /// como segundo acceso de otro, usando su correo+contraseña ya
    /// existentes -- sin pedir datos nuevos.</summary>
    [HttpPut("{usuarioObjetivoId:long}/vincular-existente/{usuarioAVincularId:long}")]
    public async Task<IActionResult> VincularParticipanteExistente(long usuarioObjetivoId, long usuarioAVincularId)
    {
        await _mediator.Send(new VincularParticipanteExistenteCommand(usuarioObjetivoId, usuarioAVincularId));
        return NoContent();
    }

    [HttpDelete("{usuarioObjetivoId:long}/vincular-existente/{usuarioVinculadoId:long}")]
    public async Task<IActionResult> DesvincularParticipante(long usuarioObjetivoId, long usuarioVinculadoId)
    {
        await _mediator.Send(new DesvincularParticipanteCommand(usuarioObjetivoId, usuarioVinculadoId));
        return NoContent();
    }
}

public sealed record AsignarParejaRequest(long? ParejaId, string? NombreEquipo);

public sealed record AgregarCredencialAlternaRequest(string Correo, string Password);

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

public sealed record RestablecerPasswordRequest(string? NuevaPassword);
