using MediatR;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Queries.Reportes.GetReporteAuditoriaPdf;
using Touchliga.Domain.Interfaces;

namespace Touchliga.Application.Handlers.Reportes.GetReporteAuditoriaPdf;

public sealed class GetReporteAuditoriaPdfQueryHandler : IRequestHandler<GetReporteAuditoriaPdfQuery, byte[]>
{
    private readonly IReportesRepository _reportes;
    private readonly IReporteAuditoriaPdfService _pdf;

    public GetReporteAuditoriaPdfQueryHandler(IReportesRepository reportes, IReporteAuditoriaPdfService pdf)
    {
        _reportes = reportes;
        _pdf = pdf;
    }

    public async Task<byte[]> Handle(GetReporteAuditoriaPdfQuery request, CancellationToken cancellationToken)
    {
        var datos = await _reportes.ObtenerDatosReporteAuditoriaAsync(request.JornadaId, cancellationToken);

        var columnas = datos.Partidos
            .Select(p => $"{p.LocalNombre} vs {p.VisitanteNombre}{(p.EsDesempate ? " (Desempate)" : "")}")
            .ToList();

        var filas = datos.Participantes.Select(participante =>
        {
            var valores = datos.Partidos.Select(partido =>
            {
                var pronostico = participante.Pronosticos.FirstOrDefault(pr => pr.PartidoId == partido.PartidoId);

                if (pronostico == null)
                    return string.Empty;

                return partido.EsDesempate && pronostico.PuntosTotalesPredichos.HasValue
                    ? $"{pronostico.EquipoGanadorNombre} (Tot: {pronostico.PuntosTotalesPredichos}, Dif: {pronostico.DiferenciaPuntosPredicha})"
                    : pronostico.EquipoGanadorNombre;
            }).ToList();

            return (participante: participante.Nombre, valores);
        }).ToList();

        return _pdf.Generar(
            $"Touchliga -- Jornada {datos.JornadaNumero}",
            "Auditoría de pronósticos registrados por todos los participantes",
            columnas,
            filas);
    }
}
