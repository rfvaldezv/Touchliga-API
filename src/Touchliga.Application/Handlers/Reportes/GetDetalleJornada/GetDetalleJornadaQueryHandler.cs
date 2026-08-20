using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Reportes.GetDetalleJornada;

namespace Touchliga.Application.Handlers.Reportes.GetDetalleJornada;

public sealed class GetDetalleJornadaQueryHandler
    : IRequestHandler<GetDetalleJornadaQuery, IReadOnlyList<DetalleJornadaDto>>
{
    private readonly IReportesRepository _repository;

    public GetDetalleJornadaQueryHandler(IReportesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<DetalleJornadaDto>> Handle(
        GetDetalleJornadaQuery request,
        CancellationToken cancellationToken)
    {
        var datos = await _repository.ObtenerDetalleJornadaAsync(request.JornadaId, cancellationToken);

        return datos.Select(d => new DetalleJornadaDto
        {
            UsuarioId = d.UsuarioId,
            Nombre = d.Nombre,
            Partidos = d.Partidos
                .Select(p => new DetallePartidoDto
                {
                    PartidoId = p.PartidoId,
                    EscudoLocalUrl = p.EscudoLocalUrl,
                    EscudoVisitanteUrl = p.EscudoVisitanteUrl,
                    LocalNombre = p.LocalNombre,
                    VisitanteNombre = p.VisitanteNombre,
                    EquipoGanadorReal = p.EquipoGanadorReal,
                    EquipoGanadorPronostico = p.EquipoGanadorPronostico,
                    EsDesempate = p.EsDesempate,
                    PuntosTotalesPredichos = p.PuntosTotalesPredichos,
                    DiferenciaPuntosPredicha = p.DiferenciaPuntosPredicha,
                    PuntosTotalesReal = p.PuntosTotalesReal,
                    DiferenciaPuntosReal = p.DiferenciaPuntosReal,
                    Puntos = p.Puntos,
                    PuntosBono = p.PuntosBono
                })
                .ToList(),
            Total = d.Total
        }).ToList();
    }
}
