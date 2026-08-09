using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.Categoria.Create;
using Touchliga.Application.Commands.Categoria.Update;
using Touchliga.Application.Commands.Categoria.Delete;

using Touchliga.Application.Queries.Categoria.Get;
using Touchliga.Application.Queries.Categoria.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar Categorias.
/// </summary>
[ApiController]
[Authorize]
[Route("api/categorias")]
public sealed class CategoriasController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoriaDto>>> GetAll()
    {
        var result = await _mediator.Send(
            new GetCategoriasQuery());

        return Ok(result);
    }

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<CategoriaDto>> Get(long id)
    {
        var result = await _mediator.Send(
            new GetCategoriaQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] CreateCategoriaCommand command)
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
        [FromBody] UpdateCategoriaCommand command)
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
            new DeleteCategoriaCommand(id));

        return NoContent();
    }
}