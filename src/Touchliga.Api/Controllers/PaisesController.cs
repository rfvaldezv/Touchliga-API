using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.Pais.Create;
using Touchliga.Application.Commands.Pais.Update;
using Touchliga.Application.Commands.Pais.Delete;

using Touchliga.Application.Queries.Pais.Get;
using Touchliga.Application.Queries.Pais.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Paises.
/// </summary>
[ApiController]
[Authorize]
[Route("api/paises")]
public sealed class PaisesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaisesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PaisDto>>> GetAll()
    {
        var result = await _mediator.Send(
            new GetPaisesQuery());

        return Ok(result);
    }

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<PaisDto>> Get(long id)
    {
        var result = await _mediator.Send(
            new GetPaisQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] CreatePaisCommand command)
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
        [FromBody] UpdatePaisCommand command)
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
            new DeletePaisCommand(id));

        return NoContent();
    }
}