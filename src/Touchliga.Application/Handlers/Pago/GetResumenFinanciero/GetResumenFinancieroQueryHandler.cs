using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Pago.GetResumenFinanciero;

namespace Touchliga.Application.Handlers.Pago.GetResumenFinanciero;

public sealed class GetResumenFinancieroQueryHandler
    : IRequestHandler<GetResumenFinancieroQuery, ResumenFinancieroDto>
{
    private readonly ITemporadaRepository _temporadas;
    private readonly IUsuarioRepository _usuarios;
    private readonly IPagoRepository _pagos;

    public GetResumenFinancieroQueryHandler(
        ITemporadaRepository temporadas,
        IUsuarioRepository usuarios,
        IPagoRepository pagos)
    {
        _temporadas = temporadas;
        _usuarios = usuarios;
        _pagos = pagos;
    }

    public async Task<ResumenFinancieroDto> Handle(
        GetResumenFinancieroQuery request,
        CancellationToken cancellationToken)
    {
        var temporada = await _temporadas.ObtenerPorIdAsync(request.TemporadaId, cancellationToken)
            ?? throw new EntityNotFoundException("Temporada");

        var usuarios = await _usuarios.ObtenerTodosAsync();
        var nombrePorUsuario = usuarios.ToDictionary(u => u.Id, u => $"{u.Nombre} {u.Apellidos}");

        var pagos = await _pagos.ObtenerPorTemporadaAsync(request.TemporadaId, cancellationToken);

        var totalPorUsuario = pagos
            .GroupBy(p => p.UsuarioId)
            .ToDictionary(g => g.Key, g => g.Sum(p => p.Monto));

        var participantesCubiertos = totalPorUsuario.Count(kv => kv.Value >= temporada.Cuota);
        var totalRecaudado = pagos.Sum(p => p.Monto);
        var totalEsperado = usuarios.Count * temporada.Cuota;

        var desglose = pagos
            .GroupBy(p => p.MetodoPago)
            .Select(g => new DesgloseMetodoPagoDto
            {
                MetodoPago = g.Key,
                Cantidad = g.Count(),
                Monto = g.Sum(p => p.Monto),
            })
            .OrderByDescending(d => d.Monto)
            .ToList();

        var ultimas = pagos
            .OrderByDescending(p => p.FechaPago)
            .Take(20)
            .Select(p => new TransaccionDto
            {
                Id = p.Id,
                UsuarioNombre = nombrePorUsuario.TryGetValue(p.UsuarioId, out var n) ? n : "Desconocido",
                Monto = p.Monto,
                MetodoPago = p.MetodoPago,
                FechaPago = p.FechaPago,
                Referencia = p.Referencia,
            })
            .ToList();

        return new ResumenFinancieroDto
        {
            Cuota = temporada.Cuota,
            TotalParticipantes = usuarios.Count,
            ParticipantesCubiertos = participantesCubiertos,
            TotalEsperado = totalEsperado,
            TotalRecaudado = totalRecaudado,
            TotalPendiente = Math.Max(0, totalEsperado - totalRecaudado),
            DesglosePorMetodo = desglose,
            UltimasTransacciones = ultimas,
        };
    }
}
