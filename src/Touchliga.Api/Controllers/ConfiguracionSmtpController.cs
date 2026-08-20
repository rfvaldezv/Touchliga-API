using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Touchliga.Application.Commands.ConfiguracionSmtp.Guardar;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.ConfiguracionSmtp.Get;

namespace Touchliga.Api.Controllers;

/// <summary>Ajustes del servidor SMTP usado para avisos a
/// participantes -- editable desde Administración, sin necesitar
/// republicar el API para cambiar proveedor o credenciales.</summary>
[ApiController]
[Route("api/configuracion/smtp")]
[Authorize(Roles = "Administrador")]
public sealed class ConfiguracionSmtpController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConfiguracionSmtpController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ConfiguracionSmtpDto?>> Obtener()
    {
        var config = await _mediator.Send(new GetConfiguracionSmtpQuery());
        return Ok(config);
    }

    [HttpPut]
    public async Task<IActionResult> Guardar([FromBody] GuardarConfiguracionSmtpCommand comando)
    {
        await _mediator.Send(comando);
        return NoContent();
    }
}
