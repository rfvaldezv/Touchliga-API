using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Estandares.GetTablaPosiciones;

namespace Touchliga.Api.Controllers;

/// <summary>
/// Tabla de posiciones.
/// </summary>
[ApiController]
[Authorize]
[Route("api/estandares")]
public sealed class EstandaresController : ControllerBase
{
    private readonly IMediator _mediator;

    public EstandaresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tabla de posiciones de una temporada.
    /// </summary>
    [HttpGet("temporada/{temporadaId:long}")]
    public async Task<ActionResult<IReadOnlyList<PosicionDto>>> GetTablaPosiciones(long temporadaId)
    {
        var result = await _mediator.Send(new GetTablaPosicionesQuery(temporadaId));

        return Ok(result);
    }
}
