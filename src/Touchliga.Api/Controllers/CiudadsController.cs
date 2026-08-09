using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.Ciudad.Create;
using Touchliga.Application.Commands.Ciudad.Update;
using Touchliga.Application.Commands.Ciudad.Delete;

using Touchliga.Application.Queries.Ciudad.Get;
using Touchliga.Application.Queries.Ciudad.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Ciudads.
/// </summary>
[ApiController]
[Authorize]
[Route("api/ciudads")]
public sealed class CiudadsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CiudadsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CiudadDto>>> GetAll()
    {
        var result = await _mediator.Send(
            new GetCiudadsQuery());

        return Ok(result);
    }

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<CiudadDto>> Get(long id)
    {
        var result = await _mediator.Send(
            new GetCiudadQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] CreateCiudadCommand command)
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
        [FromBody] UpdateCiudadCommand command)
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
            new DeleteCiudadCommand(id));

        return NoContent();
    }
}