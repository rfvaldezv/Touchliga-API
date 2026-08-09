using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Reportes.GetDetalleJornada;
using Touchliga.Application.Queries.Reportes.GetRanking;

namespace Touchliga.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/reportes")]
public sealed class ReportesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Detalle partido por partido de una jornada, por participante.</summary>
    [HttpGet("jornada/{jornadaId:long}")]
    public async Task<ActionResult<IReadOnlyList<DetalleJornadaDto>>> GetDetalleJornada(long jornadaId)
    {
        return Ok(await _mediator.Send(new GetDetalleJornadaQuery(jornadaId)));
    }

    /// <summary>Ranking de la temporada: puntos por jornada, total y % de productividad.</summary>
    [HttpGet("ranking/temporada/{temporadaId:long}")]
    public async Task<ActionResult<IReadOnlyList<RankingDto>>> GetRanking(long temporadaId)
    {
        return Ok(await _mediator.Send(new GetRankingQuery(temporadaId)));
    }
}
