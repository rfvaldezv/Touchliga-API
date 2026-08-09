using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;
using Touchliga.Application.Commands.Patrocinador.Create;
using Touchliga.Application.Commands.Patrocinador.Update;
using Touchliga.Application.Commands.Patrocinador.Delete;
using Touchliga.Application.Queries.Patrocinador.Get;
using Touchliga.Application.Queries.Patrocinador.GetAll;
using Touchliga.Application.Queries.Patrocinador.GetActivos;

namespace Touchliga.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/patrocinadores")]
public sealed class PatrocinadoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatrocinadoresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Solo los activos, para mostrar el banner rotativo — cualquier usuario.</summary>
    [HttpGet("activos")]
    public async Task<ActionResult<IReadOnlyList<PatrocinadorDto>>> GetActivos()
    {
        return Ok(await _mediator.Send(new GetPatrocinadoresActivosQuery()));
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PatrocinadorDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetPatrocinadoresQuery()));
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<PatrocinadorDto>> Get(long id)
    {
        return Ok(await _mediator.Send(new GetPatrocinadorQuery(id)));
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Post([FromBody] CreatePatrocinadorCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<long>> Put(long id, [FromBody] UpdatePatrocinadorCommand command)
    {
        if (id != command.Id) return BadRequest();

        return Ok(await _mediator.Send(command));
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeletePatrocinadorCommand(id));
        return NoContent();
    }
}
