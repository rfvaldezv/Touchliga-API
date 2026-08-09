using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Partido.GetPorJornada;

namespace Touchliga.Application.Handlers.Partido.GetPorJornada;

public sealed class GetPartidosPorJornadaQueryHandler
    : IRequestHandler<GetPartidosPorJornadaQuery, IReadOnlyList<PartidoDto>>
{
    private readonly IPartidoRepository _repository;
    private readonly ICanchaRepository _canchas;

    public GetPartidosPorJornadaQueryHandler(IPartidoRepository repository, ICanchaRepository canchas)
    {
        _repository = repository;
        _canchas = canchas;
    }

    public async Task<IReadOnlyList<PartidoDto>> Handle(
        GetPartidosPorJornadaQuery request,
        CancellationToken cancellationToken)
    {
        var partidos = await _repository.ObtenerPorJornadaAsync(request.JornadaId, cancellationToken);
        var canchas = await _canchas.ObtenerTodosAsync(cancellationToken);
        var nombresPorCanchaId = canchas.ToDictionary(c => c.Id, c => c.Nombre);

        return partidos.Select(p => new PartidoDto
        {
            Id = p.Id,
            JornadaId = p.JornadaId,
            EquipoLocalId = p.EquipoLocalId,
            EquipoVisitanteId = p.EquipoVisitanteId,
            FechaHora = p.FechaHora,
            CanchaId = p.CanchaId,
            CanchaNombre = p.CanchaId.HasValue && nombresPorCanchaId.TryGetValue(p.CanchaId.Value, out var n) ? n : null,
            GolesLocal = p.GolesLocal,
            GolesVisitante = p.GolesVisitante,
            TieneResultado = p.TieneResultado,
            EsDesempate = p.EsDesempate
        }).ToList();
    }
}
