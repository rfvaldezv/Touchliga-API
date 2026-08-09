using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Pago.GetEstatus;

namespace Touchliga.Application.Handlers.Pago.GetEstatus;

public sealed class GetEstatusPagosQueryHandler
    : IRequestHandler<GetEstatusPagosQuery, IReadOnlyList<EstatusPagoDto>>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IPagoRepository _pagos;
    private readonly ITemporadaRepository _temporadas;

    public GetEstatusPagosQueryHandler(
        IUsuarioRepository usuarios,
        IPagoRepository pagos,
        ITemporadaRepository temporadas)
    {
        _usuarios = usuarios;
        _pagos = pagos;
        _temporadas = temporadas;
    }

    public async Task<IReadOnlyList<EstatusPagoDto>> Handle(
        GetEstatusPagosQuery request,
        CancellationToken cancellationToken)
    {
        var temporada = await _temporadas.ObtenerPorIdAsync(request.TemporadaId, cancellationToken)
            ?? throw new EntityNotFoundException("Temporada");

        var usuarios = await _usuarios.ObtenerTodosAsync();
        var pagos = await _pagos.ObtenerPorTemporadaAsync(request.TemporadaId, cancellationToken);

        var totalesPorUsuario = pagos
            .GroupBy(p => p.UsuarioId)
            .ToDictionary(g => g.Key, g => g.Sum(p => p.Monto));

        return usuarios.Select(u =>
        {
            var totalPagado = totalesPorUsuario.GetValueOrDefault(u.Id, 0m);

            return new EstatusPagoDto
            {
                UsuarioId = u.Id,
                UsuarioNombre = $"{u.Nombre} {u.Apellidos}",
                Cuota = temporada.Cuota,
                TotalPagado = totalPagado,
                SaldoPendiente = Math.Max(0, temporada.Cuota - totalPagado),
                PagoCompleto = totalPagado >= temporada.Cuota && temporada.Cuota > 0,
            };
        })
        .OrderBy(e => e.PagoCompleto)
        .ThenBy(e => e.UsuarioNombre)
        .ToList();
    }
}
