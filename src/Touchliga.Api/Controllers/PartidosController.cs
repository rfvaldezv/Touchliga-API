using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;
using Touchliga.Application.Commands.Partido.Create;
using Touchliga.Application.Commands.Partido.Update;
using Touchliga.Application.Commands.Partido.Delete;
using Touchliga.Application.Commands.Partido.CapturarResultado;
using Touchliga.Application.Commands.Partido.MarcarDesempate;
using Touchliga.Application.Queries.Partido.GetPorJornada;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Partidos.
/// </summary>
[ApiController]
[Authorize]
[Route("api/partidos")]
public sealed class PartidosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PartidosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene los partidos de una jornada.
    /// </summary>
    [HttpGet("jornada/{jornadaId:long}")]
    public async Task<ActionResult<IReadOnlyList<PartidoDto>>> GetPorJornada(long jornadaId)
    {
        var result = await _mediator.Send(new GetPartidosPorJornadaQuery(jornadaId));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo partido.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post([FromBody] CreatePartidoCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(id);
    }

    /// <summary>
    /// Corrige los datos base del partido (equipos, fecha/hora, cancha).
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Put(long id, [FromBody] UpdatePartidoCommand command)
    {
        if (id != command.Id) return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Elimina un partido de forma permanente.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeletePartidoCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Captura (o corrige) el resultado real del partido.
    /// </summary>
    [Authorize(Roles = "Administrador,Capturador")]
    [HttpPost("{id:long}/resultado")]
    public async Task<IActionResult> CapturarResultado(
        long id,
        [FromBody] CapturarResultadoRequest request)
    {
        await _mediator.Send(new CapturarResultadoCommand(id, request.GolesLocal, request.GolesVisitante));

        return NoContent();
    }

    /// <summary>
    /// Marca (o desmarca) este partido como el de la caja de
    /// desempate de su jornada. Solo puede haber uno por jornada --
    /// si ya hay otro marcado, se desmarca solo.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost("{id:long}/desempate")]
    public async Task<IActionResult> MarcarDesempate(long id, [FromBody] MarcarDesempateRequest request)
    {
        await _mediator.Send(new MarcarDesempateCommand(id, request.EsDesempate));
        return NoContent();
    }
}

public sealed record CapturarResultadoRequest(int GolesLocal, int GolesVisitante);
public sealed record MarcarDesempateRequest(bool EsDesempate);
