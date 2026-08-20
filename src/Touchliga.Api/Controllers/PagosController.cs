using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Touchliga.Application.DTOs;
using Touchliga.Application.Commands.Pago.Registrar;
using Touchliga.Application.Commands.Pago.Eliminar;
using Touchliga.Application.Commands.Pago.Editar;
using Touchliga.Application.Commands.Pago.CrearSesionCheckout;
using Touchliga.Application.Commands.Pago.RegistrarDesdeWebhook;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Queries.Pago.GetEstatus;
using Touchliga.Application.Queries.Pago.GetMio;
using Touchliga.Application.Queries.Pago.GetCuentaCorriente;
using Touchliga.Application.Queries.Pago.GetResumenFinanciero;

namespace Touchliga.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/pagos")]
public sealed class PagosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPagoStripeService _stripe;

    public PagosController(IMediator mediator, IPagoStripeService stripe)
    {
        _mediator = mediator;
        _stripe = stripe;
    }

    /// <summary>Mi propio estatus de pago — cualquier usuario.</summary>
    [HttpGet("mio/temporada/{temporadaId:long}")]
    public async Task<ActionResult<ResumenPagoDto>> GetMio(long temporadaId)
    {
        return Ok(await _mediator.Send(new GetMiPagoQuery(temporadaId)));
    }

    /// <summary>Estatus de todos los participantes — solo Administrador.</summary>
    [Authorize(Roles = "Administrador")]
    [HttpGet("temporada/{temporadaId:long}/estatus")]
    public async Task<ActionResult<IReadOnlyList<EstatusPagoDto>>> GetEstatus(long temporadaId)
    {
        return Ok(await _mediator.Send(new GetEstatusPagosQuery(temporadaId)));
    }

    /// <summary>
    /// Cuenta corriente completa de un participante (todas las
    /// temporadas con cuota) — solo Administrador.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpGet("cuenta-corriente/usuario/{usuarioId:long}")]
    public async Task<ActionResult<CuentaCorrienteDto>> GetCuentaCorriente(long usuarioId)
    {
        return Ok(await _mediator.Send(new GetCuentaCorrienteQuery(usuarioId)));
    }

    /// <summary>
    /// El "escritorio" de Finanzas: recaudado vs esperado, desglose
    /// por método de pago (para distinguir Stripe de lo manual) y
    /// las últimas transacciones — solo Administrador.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [HttpGet("resumen/temporada/{temporadaId:long}")]
    public async Task<ActionResult<ResumenFinancieroDto>> GetResumenFinanciero(long temporadaId)
    {
        return Ok(await _mediator.Send(new GetResumenFinancieroQuery(temporadaId)));
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<long>> Registrar([FromBody] RegistrarPagoCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Editar(long id, [FromBody] EditarPagoCommand command)
    {
        await _mediator.Send(command with { Id = id });
        return NoContent();
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Eliminar(long id)
    {
        await _mediator.Send(new EliminarPagoCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Crea la sesión de pago con tarjeta (Stripe Checkout) para
    /// que YO pague mi propia cuota de esa temporada. Regresa la
    /// URL a la que hay que mandar al participante — la abre la app
    /// en el navegador, no requiere ningún SDK nativo.
    /// </summary>
    [HttpPost("checkout/temporada/{temporadaId:long}")]
    public async Task<ActionResult<object>> Checkout(long temporadaId, [FromQuery] string tipoPago = "Completo")
    {
        var url = await _mediator.Send(new CrearSesionCheckoutCommand(temporadaId, tipoPago));
        return Ok(new { url });
    }

    /// <summary>
    /// Stripe llama esta ruta directamente cuando confirma un cobro
    /// — nunca la llama la app. Sin [Authorize]: se valida con la
    /// firma criptográfica de Stripe, no con JWT.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        using var lector = new StreamReader(Request.Body);
        var payload = await lector.ReadToEndAsync();
        var firma = Request.Headers["Stripe-Signature"].ToString();

        var resultado = _stripe.ProcesarWebhook(payload, firma);

        if (resultado.EsPagoCompletado && resultado.UsuarioId != null && resultado.TemporadaId != null)
        {
            await _mediator.Send(new RegistrarPagoDesdeWebhookCommand(
                resultado.UsuarioId.Value,
                resultado.TemporadaId.Value,
                resultado.Monto,
                resultado.SessionId));
        }

        // Stripe espera 200 aunque el evento no nos interese (por
        // ejemplo, otros tipos de evento que no manejamos) — si le
        // regresamos error, sigue reintentando indefinidamente.
        return Ok();
    }

    /// <summary>Página simple a la que Stripe redirige tras un pago exitoso.</summary>
    [AllowAnonymous]
    [HttpGet("exitoso")]
    public ContentResult Exitoso()
    {
        return Content(PaginaResultado("¡Pago recibido! 🎉", "Ya puedes cerrar esta ventana y volver a la app."), "text/html");
    }

    /// <summary>Página simple a la que Stripe redirige si se cancela el pago.</summary>
    [AllowAnonymous]
    [HttpGet("cancelado")]
    public ContentResult Cancelado()
    {
        return Content(PaginaResultado("Pago cancelado", "No se realizó ningún cargo. Puedes cerrar esta ventana."), "text/html");
    }

    private static string PaginaResultado(string titulo, string mensaje) => $$"""
        <!DOCTYPE html>
        <html lang="es">
        <head>
          <meta charset="utf-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <title>Touchliga</title>
          <style>
            body { font-family: system-ui, sans-serif; background: #1F2841; color: white;
                    display: flex; align-items: center; justify-content: center;
                    height: 100vh; margin: 0; text-align: center; padding: 24px; }
            h1 { font-size: 24px; }
            p { color: #cbd5e1; }
          </style>
        </head>
        <body>
          <div>
            <h1>{{titulo}}</h1>
            <p>{{mensaje}}</p>
          </div>
        </body>
        </html>
        """;
}
