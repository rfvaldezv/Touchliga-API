using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Jornada.Get;

namespace Touchliga.Application.Handlers.Jornada.Get;

public sealed class GetJornadaQueryHandler : IRequestHandler<GetJornadaQuery, JornadaDto>
{
    private readonly IJornadaRepository _repository;

    public GetJornadaQueryHandler(IJornadaRepository repository)
    {
        _repository = repository;
    }

    public async Task<JornadaDto> Handle(GetJornadaQuery request, CancellationToken cancellationToken)
    {
        var j = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Jornada");

        return Map(j);
    }

    internal static JornadaDto Map(Touchliga.Domain.Entities.Jornada j) => new()
    {
        Id = j.Id,
        TemporadaId = j.TemporadaId,
        Codigo = j.Codigo,
        Nombre = j.Nombre,
        Descripcion = j.Descripcion ?? string.Empty,
        Numero = j.Numero,
        FechaCierre = j.FechaCierre,
        Cerrada = j.Cerrada,
        Activo = j.Activo
    };
}
