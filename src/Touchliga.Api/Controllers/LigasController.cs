using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.Liga.Create;
using Touchliga.Application.Commands.Liga.Update;
using Touchliga.Application.Commands.Liga.Delete;

using Touchliga.Application.Queries.Liga.Get;
using Touchliga.Application.Queries.Liga.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Ligas.
/// </summary>
[ApiController]
[Authorize]
[Route("api/ligas")]
public sealed class LigasController : ControllerBase
{
    private readonly IMediator _mediator;

    public LigasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LigaDto>>> GetAll()
    {
        var result = await _mediator.Send(
            new GetLigasQuery());

        return Ok(result);
    }

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<LigaDto>> Get(long id)
    {
        var result = await _mediator.Send(
            new GetLigaQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] CreateLigaCommand command)
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
        [FromBody] UpdateLigaCommand command)
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
            new DeleteLigaCommand(id));

        return NoContent();
    }
}