using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Common.Utils;
using Touchliga.Application.Queries.Premio.GetGanadoresJornada;

namespace Touchliga.Application.Handlers.Premio.GetGanadoresJornada;

public sealed class GetGanadoresJornadaQueryHandler
    : IRequestHandler<GetGanadoresJornadaQuery, IReadOnlyList<GanadorPremioDto>>
{
    private readonly IJornadaRepository _jornadas;
    private readonly IReportesRepository _reportes;
    private readonly IConfiguracionPremioRepository _premios;
    private readonly IPremioOtorgadoRepository _decisiones;

    public GetGanadoresJornadaQueryHandler(
        IJornadaRepository jornadas,
        IReportesRepository reportes,
        IConfiguracionPremioRepository premios,
        IPremioOtorgadoRepository decisiones)
    {
        _jornadas = jornadas;
        _reportes = reportes;
        _premios = premios;
        _decisiones = decisiones;
    }

    public async Task<IReadOnlyList<GanadorPremioDto>> Handle(
        GetGanadoresJornadaQuery request,
        CancellationToken cancellationToken)
    {
        var jornada = await _jornadas.ObtenerPorIdAsync(request.JornadaId, cancellationToken)
            ?? throw new EntityNotFoundException("Jornada");

        var detalle = await _reportes.ObtenerDetalleJornadaAsync(request.JornadaId, cancellationToken);
        var premiosConfigurados = await _premios.ObtenerPorTemporadaYAmbitoAsync(
            jornada.TemporadaId, "Jornada", cancellationToken);

        // ObtenerDetalleJornadaAsync ya viene ordenado descendente por Total.
        var participantes = detalle
            .Select(d => (d.UsuarioId, d.Nombre, Puntos: d.Total))
            .ToList();

        var sugerencias = CalculadoraPremios.Calcular(participantes, premiosConfigurados);

        var decisiones = await _decisiones.ObtenerPorReferenciaAsync(
            "Jornada", request.JornadaId, cancellationToken);
        var decisionesPorUsuario = decisiones.ToDictionary(d => d.UsuarioId);

        foreach (var grupo in sugerencias)
        {
            foreach (var participante in grupo.Participantes)
            {
                if (decisionesPorUsuario.TryGetValue(participante.UsuarioId, out var decision))
                {
                    participante.Estado = decision.Estado;
                    participante.MontoAjustado = decision.MontoAjustado;
                    participante.Motivo = decision.Motivo;
                }
            }
        }

        return sugerencias;
    }
}
