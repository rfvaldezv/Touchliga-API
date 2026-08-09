using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Common.Utils;
using Touchliga.Application.Queries.Premio.GetGanadoresFinales;

namespace Touchliga.Application.Handlers.Premio.GetGanadoresFinales;

public sealed class GetGanadoresFinalesQueryHandler
    : IRequestHandler<GetGanadoresFinalesQuery, IReadOnlyList<GanadorPremioDto>>
{
    private readonly IReportesRepository _reportes;
    private readonly IConfiguracionPremioRepository _premios;
    private readonly IPremioOtorgadoRepository _decisiones;

    public GetGanadoresFinalesQueryHandler(
        IReportesRepository reportes,
        IConfiguracionPremioRepository premios,
        IPremioOtorgadoRepository decisiones)
    {
        _reportes = reportes;
        _premios = premios;
        _decisiones = decisiones;
    }

    public async Task<IReadOnlyList<GanadorPremioDto>> Handle(
        GetGanadoresFinalesQuery request,
        CancellationToken cancellationToken)
    {
        var ranking = await _reportes.ObtenerRankingAsync(request.TemporadaId, cancellationToken);
        var premiosConfigurados = await _premios.ObtenerPorTemporadaYAmbitoAsync(
            request.TemporadaId, "Final", cancellationToken);

        // ObtenerRankingAsync ya viene ordenado descendente por TotalPuntos.
        var participantes = ranking
            .Select(r => (r.UsuarioId, r.Nombre, Puntos: r.TotalPuntos))
            .ToList();

        var sugerencias = CalculadoraPremios.Calcular(participantes, premiosConfigurados);

        var decisiones = await _decisiones.ObtenerPorReferenciaAsync(
            "Final", request.TemporadaId, cancellationToken);
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
