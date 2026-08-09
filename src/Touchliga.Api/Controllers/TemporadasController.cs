using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.Temporada.Create;
using Touchliga.Application.Commands.Temporada.Update;
using Touchliga.Application.Commands.Temporada.Delete;

using Touchliga.Application.Queries.Temporada.Get;
using Touchliga.Application.Queries.Temporada.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Temporadas.
/// </summary>
[ApiController]
[Authorize]
[Route("api/temporadas")]
public sealed class TemporadasController : ControllerBase
{
    private readonly IMediator _mediator;

    public TemporadasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TemporadaDto>>> GetAll([FromQuery] long? ligaId)
    {
        var result = await _mediator.Send(
            new GetTemporadasQuery(ligaId));

        return Ok(result);
    }

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<TemporadaDto>> Get(long id)
    {
        var result = await _mediator.Send(
            new GetTemporadaQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] CreateTemporadaCommand command)
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
        [FromBody] UpdateTemporadaCommand command)
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
            new DeleteTemporadaCommand(id));

        return NoContent();
    }
}