using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Liga.GetAll;

namespace Touchliga.Application.Handlers.Liga.GetAll;

public sealed class GetLigasQueryHandler : IRequestHandler<GetLigasQuery, IReadOnlyList<LigaDto>>
{
    private readonly ILigaRepository _repository;

    public GetLigasQueryHandler(
        ILigaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<LigaDto>> Handle(
        GetLigasQuery request,
        CancellationToken cancellationToken)
    {
        var ligas = await _repository.ObtenerTodosAsync(cancellationToken);

        return ligas
            .Select(liga => new LigaDto
            {
                Id = liga.Id,
                Codigo = liga.Codigo,
                Nombre = liga.Nombre,
                Descripcion = liga.Descripcion ?? string.Empty,
                Activo = liga.Activo
            })
            .ToList();
    }
}
