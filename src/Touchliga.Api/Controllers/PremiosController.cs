using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;
using Touchliga.Application.Commands.Premio.GuardarConfiguracion;
using Touchliga.Application.Commands.Premio.Decidir;
using Touchliga.Application.Queries.Premio.GetConfiguracion;
using Touchliga.Application.Queries.Premio.GetGanadoresJornada;
using Touchliga.Application.Queries.Premio.GetGanadoresFinales;

namespace Touchliga.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/premios")]
public sealed class PremiosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PremiosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Configuración de premios (montos/regalos por posición) —
    /// cualquier usuario puede consultarla, por transparencia total.</summary>
    [HttpGet("configuracion/temporada/{temporadaId:long}/ambito/{ambito}")]
    public async Task<ActionResult<IReadOnlyList<ConfiguracionPremioDto>>> GetConfiguracion(
        long temporadaId, string ambito)
    {
        return Ok(await _mediator.Send(new GetConfiguracionPremiosQuery(temporadaId, ambito)));
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost("configuracion")]
    public async Task<IActionResult> GuardarConfiguracion([FromBody] GuardarConfiguracionPremiosCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>Ganadores calculados de una jornada específica.</summary>
    [HttpGet("ganadores/jornada/{jornadaId:long}")]
    public async Task<ActionResult<IReadOnlyList<GanadorPremioDto>>> GetGanadoresJornada(long jornadaId)
    {
        return Ok(await _mediator.Send(new GetGanadoresJornadaQuery(jornadaId)));
    }

    /// <summary>Ganadores finales calculados de toda la temporada (acumulado).</summary>
    [HttpGet("ganadores/temporada/{temporadaId:long}")]
    public async Task<ActionResult<IReadOnlyList<GanadorPremioDto>>> GetGanadoresFinales(long temporadaId)
    {
        return Ok(await _mediator.Send(new GetGanadoresFinalesQuery(temporadaId)));
    }

    /// <summary>
    /// El responsable de finanzas decide sobre un premio sugerido:
    /// lo aprueba (tal cual o con un monto ajustado) o lo niega (por
    /// ejemplo, por mala actitud). El cálculo automático nunca se da
    /// por aprobado solo — siempre requiere esta decisión explícita.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost("decidir")]
    public async Task<IActionResult> Decidir([FromBody] DecidirPremioCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
