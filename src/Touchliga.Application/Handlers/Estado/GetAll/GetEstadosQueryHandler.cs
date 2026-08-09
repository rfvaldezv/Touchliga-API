using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Estado.GetAll;

namespace Touchliga.Application.Handlers.Estado.GetAll;

public sealed class GetEstadosQueryHandler : IRequestHandler<GetEstadosQuery, IReadOnlyList<EstadoDto>>
{
    private readonly IEstadoRepository _repository;
    private readonly IPaisRepository _paises;

    public GetEstadosQueryHandler(
        IEstadoRepository repository,
        IPaisRepository paises)
    {
        _repository = repository;
        _paises = paises;
    }

    public async Task<IReadOnlyList<EstadoDto>> Handle(
        GetEstadosQuery request,
        CancellationToken cancellationToken)
    {
        var entidades = await _repository.ObtenerTodosAsync(cancellationToken);
        var paises = await _paises.ObtenerTodosAsync(cancellationToken);
        var nombresPorPaisId = paises.ToDictionary(p => p.Id, p => p.Nombre);

        return entidades
            .Select(entidad => new EstadoDto
            {
                Id = entidad.Id,
                Codigo = entidad.Codigo,
                Nombre = entidad.Nombre,
                Descripcion = entidad.Descripcion ?? string.Empty,
                PaisId = entidad.PaisId,
                PaisNombre = nombresPorPaisId.TryGetValue(entidad.PaisId, out var n) ? n : string.Empty,
                Activo = entidad.Activo
            })
            .ToList();
    }
}
