using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Estadisticas.GetEstadisticasParticipante;

namespace Touchliga.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/estadisticas")]
public sealed class EstadisticasController : ControllerBase
{
    private readonly IMediator _mediator;

    public EstadisticasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Mis estadísticas personales (racha, tendencia, podio,
    /// aciertos, jugada más loca) — siempre las del usuario autenticado.</summary>
    [HttpGet("participante/temporada/{temporadaId:long}")]
    public async Task<ActionResult<EstadisticasParticipanteDto>> GetMias(long temporadaId)
    {
        return Ok(await _mediator.Send(new GetEstadisticasParticipanteQuery(temporadaId)));
    }
}
