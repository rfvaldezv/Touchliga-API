using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Reportes.GetDetalleJornada;
using Touchliga.Application.Queries.Reportes.GetRanking;
using Touchliga.Application.Queries.Reportes.GetParticipantesPendientes;
using Touchliga.Application.Queries.Reportes.GetReporteAuditoriaPdf;

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

    /// <summary>Participantes activos que aún no completan sus
    /// pronósticos de una jornada -- para saber a quién recordarle.</summary>
    [HttpGet("pendientes/jornada/{jornadaId:long}")]
    public async Task<ActionResult<List<ParticipantePendienteDto>>> GetParticipantesPendientes(long jornadaId)
    {
        return Ok(await _mediator.Send(new GetParticipantesPendientesQuery(jornadaId)));
    }

    /// <summary>PDF de auditoría de una jornada: tabla con cada
    /// participante y sus pronósticos de cada partido -- para
    /// compartir en el grupo de WhatsApp.</summary>
    [HttpGet("jornada/{jornadaId:long}/pdf-auditoria")]
    public async Task<IActionResult> GetReporteAuditoriaPdf(long jornadaId)
    {
        var pdf = await _mediator.Send(new GetReporteAuditoriaPdfQuery(jornadaId));
        return File(pdf, "application/pdf", $"Jornada_{jornadaId}_Auditoria.pdf");
    }
}
