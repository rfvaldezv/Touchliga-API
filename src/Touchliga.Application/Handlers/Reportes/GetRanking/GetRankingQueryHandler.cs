using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Reportes.GetRanking;

namespace Touchliga.Application.Handlers.Reportes.GetRanking;

public sealed class GetRankingQueryHandler : IRequestHandler<GetRankingQuery, IReadOnlyList<RankingDto>>
{
    private readonly IReportesRepository _repository;

    public GetRankingQueryHandler(IReportesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<RankingDto>> Handle(
        GetRankingQuery request,
        CancellationToken cancellationToken)
    {
        var datos = await _repository.ObtenerRankingAsync(request.TemporadaId, cancellationToken);

        return datos.Select(d => new RankingDto
        {
            UsuarioId = d.UsuarioId,
            Nombre = d.Nombre,
            TienePareja = d.TienePareja,
            NombreEquipo = d.NombreEquipo,
            Jornadas = d.Jornadas
                .Select(j => new PuntosPorJornadaDto
                {
                    JornadaId = j.JornadaId,
                    Numero = j.Numero,
                    Puntos = j.Puntos,
                    Calificados = j.Calificados,
                    TodosLosPartidosConResultado = j.TodosLosPartidosConResultado
                })
                .ToList(),
            TotalPuntos = d.TotalPuntos,
            PorcentajeProductividad = d.PorcentajeProductividad
        }).ToList();
    }
}
