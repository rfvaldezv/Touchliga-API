using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.Users.DTOs;
using Touchliga.Application.Users.Queries.GetMiPerfil;
using Touchliga.Application.Users.Commands.ActualizarPerfil;

namespace Touchliga.Api.Controllers;

/// <summary>
/// Perfil del usuario autenticado (a diferencia de UsuariosController,
/// aquí no se requiere rol de Administrador — cualquier usuario puede
/// ver y completar su propio perfil).
/// </summary>
[ApiController]
[Authorize]
[Route("api/perfil")]
public sealed class PerfilController : ControllerBase
{
    private readonly IMediator _mediator;

    public PerfilController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<UsuarioAdminDto>> Get()
    {
        return Ok(await _mediator.Send(new GetMiPerfilQuery()));
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] ActualizarPerfilCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
