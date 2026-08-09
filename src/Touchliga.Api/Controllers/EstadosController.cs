using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.Estado.Create;
using Touchliga.Application.Commands.Estado.Update;
using Touchliga.Application.Commands.Estado.Delete;

using Touchliga.Application.Queries.Estado.Get;
using Touchliga.Application.Queries.Estado.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Estados.
/// </summary>
[ApiController]
[Authorize]
[Route("api/estados")]
public sealed class EstadosController : ControllerBase
{
    private readonly IMediator _mediator;

    public EstadosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EstadoDto>>> GetAll()
    {
        var result = await _mediator.Send(
            new GetEstadosQuery());

        return Ok(result);
    }

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<EstadoDto>> Get(long id)
    {
        var result = await _mediator.Send(
            new GetEstadoQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] CreateEstadoCommand command)
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
        [FromBody] UpdateEstadoCommand command)
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
            new DeleteEstadoCommand(id));

        return NoContent();
    }
}