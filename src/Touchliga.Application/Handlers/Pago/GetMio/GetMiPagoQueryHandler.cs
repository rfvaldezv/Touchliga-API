using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Pago.GetMio;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Pago.GetMio;

public sealed class GetMiPagoQueryHandler : IRequestHandler<GetMiPagoQuery, ResumenPagoDto>
{
    private readonly IPagoRepository _pagos;
    private readonly ITemporadaRepository _temporadas;
    private readonly ICurrentUserService _currentUser;

    public GetMiPagoQueryHandler(
        IPagoRepository pagos,
        ITemporadaRepository temporadas,
        ICurrentUserService currentUser)
    {
        _pagos = pagos;
        _temporadas = temporadas;
        _currentUser = currentUser;
    }

    public async Task<ResumenPagoDto> Handle(GetMiPagoQuery request, CancellationToken cancellationToken)
    {
        var temporada = await _temporadas.ObtenerPorIdAsync(request.TemporadaId, cancellationToken)
            ?? throw new EntityNotFoundException("Temporada");

        var pagos = await _pagos.ObtenerListaPorUsuarioYTemporadaAsync(
            _currentUser.UserId, request.TemporadaId, cancellationToken);

        var totalPagado = pagos.Sum(p => p.Monto);

        return new ResumenPagoDto
        {
            Cuota = temporada.Cuota,
            TotalPagado = totalPagado,
            SaldoPendiente = Math.Max(0, temporada.Cuota - totalPagado),
            PagoCompleto = totalPagado >= temporada.Cuota && temporada.Cuota > 0,
            Pagos = pagos.Select(p => new PagoDto
            {
                Id = p.Id,
                UsuarioId = p.UsuarioId,
                TemporadaId = p.TemporadaId,
                Monto = p.Monto,
                MetodoPago = p.MetodoPago,
                FechaPago = p.FechaPago,
                Referencia = p.Referencia,
            }).ToList(),
        };
    }
}
