using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Temporada.Get;

namespace Touchliga.Application.Handlers.Temporada.Get;

public sealed class GetTemporadaQueryHandler : IRequestHandler<GetTemporadaQuery, TemporadaDto>
{
    private readonly ITemporadaRepository _repository;

    public GetTemporadaQueryHandler(ITemporadaRepository repository)
    {
        _repository = repository;
    }

    public async Task<TemporadaDto> Handle(GetTemporadaQuery request, CancellationToken cancellationToken)
    {
        var t = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Temporada");

        return new TemporadaDto
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
        };
    }
}
