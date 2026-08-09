using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;
using Touchliga.Application.Commands.Pronostico.Guardar;
using Touchliga.Application.Commands.Pronostico.GuardarLote;
using Touchliga.Application.Queries.Pronostico.GetMiosPorJornada;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para capturar y consultar Pronósticos.
/// </summary>
[ApiController]
[Authorize]
[Route("api/pronosticos")]
public sealed class PronosticosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PronosticosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene mis pronósticos capturados en una jornada.
    /// </summary>
    [HttpGet("mios/jornada/{jornadaId:long}")]
    public async Task<ActionResult<IReadOnlyList<PronosticoDto>>> GetMisPronosticos(long jornadaId)
    {
        var result = await _mediator.Send(new GetMisPronosticosPorJornadaQuery(jornadaId));

        return Ok(result);
    }

    /// <summary>
    /// Crea o actualiza mi pronóstico para un partido.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<long>> Guardar([FromBody] GuardarPronosticoCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(id);
    }

    /// <summary>
    /// Guarda todos mis pronósticos de una jornada en un solo paso.
    /// Si con esto quedan cubiertos todos los partidos, se manda un
    /// correo de confirmación. Devuelve true si la jornada quedó
    /// completa.
    /// </summary>
    [HttpPost("jornada/{jornadaId:long}/lote")]
    public async Task<ActionResult<bool>> GuardarLote(
        long jornadaId,
        [FromBody] List<PronosticoLoteItem> pronosticos)
    {
        var completa = await _mediator.Send(new GuardarPronosticosLoteCommand(jornadaId, pronosticos));

        return Ok(completa);
    }
}
