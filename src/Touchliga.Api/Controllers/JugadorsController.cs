using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.Jugador.Create;
using Touchliga.Application.Commands.Jugador.Update;
using Touchliga.Application.Commands.Jugador.Delete;

using Touchliga.Application.Queries.Jugador.Get;
using Touchliga.Application.Queries.Jugador.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Jugadors.
/// </summary>
[ApiController]
[Authorize]
[Route("api/jugadors")]
public sealed class JugadorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JugadorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<JugadorDto>>> GetAll()
    {
        var result = await _mediator.Send(
            new GetJugadorsQuery());

        return Ok(result);
    }

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<JugadorDto>> Get(long id)
    {
        var result = await _mediator.Send(
            new GetJugadorQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] CreateJugadorCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(id);
    }

    /// <summary>
    /// Actualiza un registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<long>> Put(
        long id,
        [FromBody] UpdateJugadorCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Elimina un registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(
            new DeleteJugadorCommand(id));

        return NoContent();
    }
}