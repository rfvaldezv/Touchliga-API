using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.Equipo.Create;
using Touchliga.Application.Commands.Equipo.Update;
using Touchliga.Application.Commands.Equipo.Delete;

using Touchliga.Application.Queries.Equipo.Get;
using Touchliga.Application.Queries.Equipo.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Equipos.
/// </summary>
[ApiController]
[Authorize]
[Route("api/equipos")]
public sealed class EquiposController : ControllerBase
{
    private readonly IMediator _mediator;

    public EquiposController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EquipoDto>>> GetAll()
    {
        var result = await _mediator.Send(
            new GetEquiposQuery());

        return Ok(result);
    }

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<EquipoDto>> Get(long id)
    {
        var result = await _mediator.Send(
            new GetEquipoQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] CreateEquipoCommand command)
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
        [FromBody] UpdateEquipoCommand command)
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
            new DeleteEquipoCommand(id));

        return NoContent();
    }
}