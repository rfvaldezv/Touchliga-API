using Touchliga.ModuleGenerator.Core;

namespace Touchliga.ModuleGenerator.Roslyn;

public static class ControllerSourceBuilder
{
    public static string Build(ModuleDefinition module)
    {
        return
$@"using MediatR;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;

using Touchliga.Application.Commands.{module.Entity}.Create;
using Touchliga.Application.Commands.{module.Entity}.Update;
using Touchliga.Application.Commands.{module.Entity}.Delete;

using Touchliga.Application.Queries.{module.Entity}.Get;
using Touchliga.Application.Queries.{module.Entity}.GetAll;

namespace Touchliga.Api.Controllers;

/// <summary>
/// API para administrar {module.EntityPlural}.
/// </summary>
[ApiController]
[Route(""{module.Route}"")]
public sealed class {module.ControllerName} : ControllerBase
{{
    private readonly IMediator _mediator;

    public {module.ControllerName}(IMediator mediator)
    {{
        _mediator = mediator;
    }}

    /// <summary>
    /// Obtiene todos los registros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<{module.Entity}Dto>>> GetAll()
    {{
        var result = await _mediator.Send(
            new Get{module.EntityPlural}Query());

        return Ok(result);
    }}

    /// <summary>
    /// Obtiene un registro por Id.
    /// </summary>
    [HttpGet(""{{id:long}}"")]
    public async Task<ActionResult<{module.Entity}Dto>> Get(long id)
    {{
        var result = await _mediator.Send(
            new Get{module.Entity}Query(id));

        return Ok(result);
    }}

    /// <summary>
    /// Crea un nuevo registro.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<long>> Post(
        [FromBody] Create{module.Entity}Command command)
    {{
        var id = await _mediator.Send(command);

        return Ok(id);
    }}

    /// <summary>
    /// Actualiza un registro.
    /// </summary>
    [HttpPut(""{{id:long}}"")]
    public async Task<ActionResult<long>> Put(
        long id,
        [FromBody] Update{module.Entity}Command command)
    {{
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);

        return Ok(result);
    }}

    /// <summary>
    /// Elimina un registro.
    /// </summary>
    [HttpDelete(""{{id:long}}"")]
    public async Task<IActionResult> Delete(long id)
    {{
        await _mediator.Send(
            new Delete{module.Entity}Command(id));

        return NoContent();
    }}
}}
";
    }
}
