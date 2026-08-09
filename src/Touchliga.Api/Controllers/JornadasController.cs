using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.Jornada.Create;
using Touchliga.Application.Commands.Jornada.Update;
using Touchliga.Application.Commands.Jornada.Delete;
using Touchliga.Application.Commands.Jornada.Cerrar;
using Touchliga.Application.Commands.Jornada.Abrir;

using Touchliga.Application.Queries.Jornada.Get;
using Touchliga.Application.Queries.Jornada.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Jornadas.
/// </summary>
[ApiController]
[Authorize]
[Route("api/jornadas")]
public sealed class JornadasController : ControllerBase
{
    private readonly IMediator _mediator;

    public JornadasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<JornadaDto>>> GetAll([FromQuery] long? temporadaId)
    {
        var result = await _mediator.Send(
            new GetJornadasQuery(temporadaId));

        return Ok(result);
    }

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<JornadaDto>> Get(long id)
    {
        var result = await _mediator.Send(
            new GetJornadaQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] CreateJornadaCommand command)
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
        [FromBody] UpdateJornadaCommand command)
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
            new DeleteJornadaCommand(id));

        return NoContent();
    }

    /// <summary>
    /// Cierra la jornada: ya no se pueden capturar ni editar
    /// pronósticos de sus partidos, y se calculan los puntos de
    /// todos los pronósticos con base en los resultados capturados.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost("{id:long}/cerrar")]
    public async Task<IActionResult> Cerrar(long id)
    {
        await _mediator.Send(new CerrarJornadaCommand(id));

        return NoContent();
    }

    /// <summary>
    /// Reabre una jornada ya cerrada, para poder corregir un
    /// pronóstico o resultado capturado por error. Se puede volver
    /// a cerrar normalmente después.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost("{id:long}/abrir")]
    public async Task<IActionResult> Abrir(long id)
    {
        await _mediator.Send(new AbrirJornadaCommand(id));

        return NoContent();
    }
}