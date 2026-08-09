using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.Communication.DTOs;
using Touchliga.Application.Communication.Commands.CrearAnuncio;
using Touchliga.Application.Communication.Commands.EditarAnuncio;
using Touchliga.Application.Communication.Commands.EliminarAnuncio;
using Touchliga.Application.Communication.Commands.ReaccionarAnuncio;
using Touchliga.Application.Communication.Queries.GetAnuncios;

namespace Touchliga.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/anuncios")]
public sealed class AnunciosController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnunciosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AnuncioDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetAnunciosQuery()));
    }

    [Authorize(Roles = "Administrador,Capturador")]
    [HttpPost]
    public async Task<ActionResult<long>> Crear([FromBody] CrearAnuncioCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Roles = "Administrador,Capturador")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Editar(long id, [FromBody] EditarAnuncioCommand command)
    {
        if (id != command.Id) return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    [Authorize(Roles = "Administrador,Capturador")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Eliminar(long id)
    {
        await _mediator.Send(new EliminarAnuncioCommand(id));
        return NoContent();
    }

    /// <summary>Cualquier participante puede reaccionar — no requiere rol especial.</summary>
    [HttpPost("{id:long}/reaccionar")]
    public async Task<IActionResult> Reaccionar(long id, [FromBody] ReaccionarRequest body)
    {
        await _mediator.Send(new ReaccionarAnuncioCommand(id, body.Emoji));
        return NoContent();
    }
}

public sealed record ReaccionarRequest(string Emoji);
