using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Temporada.GetAll;

namespace Touchliga.Application.Handlers.Temporada.GetAll;

public sealed class GetTemporadasQueryHandler : IRequestHandler<GetTemporadasQuery, IReadOnlyList<TemporadaDto>>
{
    private readonly ITemporadaRepository _repository;

    public GetTemporadasQueryHandler(ITemporadaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<TemporadaDto>> Handle(GetTemporadasQuery request, CancellationToken cancellationToken)
    {
        var temporadas = await _repository.ObtenerTodosAsync(cancellationToken);

        if (request.LigaId.HasValue)
        {
            temporadas = temporadas.Where(t => t.LigaId == request.LigaId.Value).ToList();
        }

        return temporadas.Select(t => new TemporadaDto
        {
            Id = t.Id,
            LigaId = t.LigaId,
            Codigo = t.Codigo,
            Nombre = t.Nombre,
            Descripcion = t.Descripcion ?? string.Empty,
            FechaInicio = t.FechaInicio,
            FechaFin = t.FechaFin,
            Cuota = t.Cuota,
            Activo = t.Activo
        }).ToList();
    }
}
