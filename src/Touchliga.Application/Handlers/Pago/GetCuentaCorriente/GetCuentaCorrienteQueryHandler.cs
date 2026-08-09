using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Pago.GetCuentaCorriente;

namespace Touchliga.Application.Handlers.Pago.GetCuentaCorriente;

public sealed class GetCuentaCorrienteQueryHandler
    : IRequestHandler<GetCuentaCorrienteQuery, CuentaCorrienteDto>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ITemporadaRepository _temporadas;
    private readonly IPagoRepository _pagos;

    public GetCuentaCorrienteQueryHandler(
        IUsuarioRepository usuarios,
        ITemporadaRepository temporadas,
        IPagoRepository pagos)
    {
        _usuarios = usuarios;
        _temporadas = temporadas;
        _pagos = pagos;
    }

    public async Task<CuentaCorrienteDto> Handle(
        GetCuentaCorrienteQuery request,
        CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId)
            ?? throw new EntityNotFoundException("Usuario");

        var todasLasTemporadas = await _temporadas.ObtenerTodosAsync(cancellationToken);
        var temporadasConCuota = todasLasTemporadas.Where(t => t.Cuota > 0).ToList();

        var pagosDelUsuario = await _pagos.ObtenerPorUsuarioAsync(request.UsuarioId, cancellationToken);
        var pagosPorTemporada = pagosDelUsuario.GroupBy(p => p.TemporadaId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var temporadasDto = temporadasConCuota.Select(t =>
        {
            var pagosDeEsta = pagosPorTemporada.GetValueOrDefault(t.Id, []);
            var totalPagado = pagosDeEsta.Sum(p => p.Monto);

            return new CuentaCorrienteTemporadaDto
            {
                TemporadaId = t.Id,
                TemporadaNombre = t.Nombre,
                Cuota = t.Cuota,
                TotalPagado = totalPagado,
                SaldoPendiente = Math.Max(0, t.Cuota - totalPagado),
                PagoCompleto = totalPagado >= t.Cuota,
                Pagos = pagosDeEsta.Select(p => new PagoDto
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    TemporadaId = p.TemporadaId,
                    Monto = p.Monto,
                    MetodoPago = p.MetodoPago,
                    FechaPago = p.FechaPago,
                    Referencia = p.Referencia,
                }).OrderBy(p => p.FechaPago).ToList(),
            };
        })
        .OrderByDescending(t => t.TemporadaId)
        .ToList();

        return new CuentaCorrienteDto
        {
            UsuarioId = usuario.Id,
            UsuarioNombre = $"{usuario.Nombre} {usuario.Apellidos}",
            TotalAdeudado = temporadasDto.Sum(t => t.Cuota),
            TotalPagado = temporadasDto.Sum(t => t.TotalPagado),
            SaldoTotal = temporadasDto.Sum(t => t.SaldoPendiente),
            Temporadas = temporadasDto,
        };
    }
}
