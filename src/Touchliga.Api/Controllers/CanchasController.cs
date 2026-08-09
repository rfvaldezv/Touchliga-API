using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.Cancha.Create;
using Touchliga.Application.Commands.Cancha.Update;
using Touchliga.Application.Commands.Cancha.Delete;

using Touchliga.Application.Queries.Cancha.Get;
using Touchliga.Application.Queries.Cancha.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Canchas.
/// </summary>
[ApiController]
[Authorize]
[Route("api/canchas")]
public sealed class CanchasController : ControllerBase
{
    private readonly IMediator _mediator;

    public CanchasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CanchaDto>>> GetAll()
    {
        var result = await _mediator.Send(
            new GetCanchasQuery());

        return Ok(result);
    }

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<CanchaDto>> Get(long id)
    {
        var result = await _mediator.Send(
            new GetCanchaQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] CreateCanchaCommand command)
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
        [FromBody] UpdateCanchaCommand command)
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
            new DeleteCanchaCommand(id));

        return NoContent();
    }
}