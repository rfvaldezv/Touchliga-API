using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.Commands.PushToken.Registrar;
using Touchliga.Application.Commands.PushToken.Eliminar;

namespace Touchliga.Api.Controllers;

/// <summary>Registro de dispositivos para notificaciones push.</summary>
[ApiController]
[Authorize]
[Route("api/notificaciones")]
public sealed class NotificacionesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificacionesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("dispositivo")]
    public async Task<IActionResult> RegistrarDispositivo([FromBody] RegistrarPushTokenCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("dispositivo/{token}")]
    public async Task<IActionResult> EliminarDispositivo(string token)
    {
        await _mediator.Send(new EliminarPushTokenCommand(token));
        return NoContent();
    }
}
