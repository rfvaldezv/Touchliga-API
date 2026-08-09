using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.Communication.DTOs;
using Touchliga.Application.Communication.Commands.EnviarMensaje;
using Touchliga.Application.Communication.Commands.EditarMensaje;
using Touchliga.Application.Communication.Commands.EliminarMensaje;
using Touchliga.Application.Communication.Commands.MarcarConversacionLeida;
using Touchliga.Application.Communication.Queries.GetConversacion;
using Touchliga.Application.Communication.Queries.GetMisContactos;
using Touchliga.Application.Communication.Queries.GetOrganizadores;
using Touchliga.Application.Communication.Queries.GetTodosLosParticipantes;

namespace Touchliga.Api.Controllers;

/// <summary>
/// Mensajes directos entre usuarios. Autoservicio: no requiere rol,
/// cualquier usuario puede escribirle a cualquier otro (el remitente
/// siempre es el usuario autenticado, nunca se recibe por parámetro).
/// </summary>
[ApiController]
[Authorize]
[Route("api/mensajes")]
public sealed class MensajesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MensajesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("contactos")]
    public async Task<ActionResult<IReadOnlyList<ContactoDto>>> GetMisContactos()
    {
        return Ok(await _mediator.Send(new GetMisContactosQuery()));
    }

    [HttpGet("organizadores")]
    public async Task<ActionResult<IReadOnlyList<ContactoDto>>> GetOrganizadores()
    {
        return Ok(await _mediator.Send(new GetOrganizadoresQuery()));
    }

    [HttpGet("participantes")]
    public async Task<ActionResult<IReadOnlyList<ContactoDto>>> GetTodosLosParticipantes()
    {
        return Ok(await _mediator.Send(new GetTodosLosParticipantesQuery()));
    }

    [HttpGet("conversacion/{otroUsuarioId:long}")]
    public async Task<ActionResult<IReadOnlyList<MensajeDto>>> GetConversacion(long otroUsuarioId)
    {
        return Ok(await _mediator.Send(new GetConversacionQuery(otroUsuarioId)));
    }

    [HttpPost]
    public async Task<ActionResult<long>> Enviar([FromBody] EnviarMensajeCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Editar(long id, [FromBody] EditarMensajeCommand command)
    {
        if (id != command.Id) return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Eliminar(long id)
    {
        await _mediator.Send(new EliminarMensajeCommand(id));
        return NoContent();
    }

    [HttpPost("conversacion/{otroUsuarioId:long}/marcar-leida")]
    public async Task<IActionResult> MarcarLeida(long otroUsuarioId)
    {
        await _mediator.Send(new MarcarConversacionLeidaCommand(otroUsuarioId));
        return NoContent();
    }
}
