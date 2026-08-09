using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Ciudad.GetAll;

namespace Touchliga.Application.Handlers.Ciudad.GetAll;

public sealed class GetCiudadsQueryHandler : IRequestHandler<GetCiudadsQuery, IReadOnlyList<CiudadDto>>
{
    private readonly ICiudadRepository _repository;
    private readonly IPaisRepository _paises;
    private readonly IEstadoRepository _estados;

    public GetCiudadsQueryHandler(
        ICiudadRepository repository,
        IPaisRepository paises,
        IEstadoRepository estados)
    {
        _repository = repository;
        _paises = paises;
        _estados = estados;
    }

    public async Task<IReadOnlyList<CiudadDto>> Handle(
        GetCiudadsQuery request,
        CancellationToken cancellationToken)
    {
        var entidades = await _repository.ObtenerTodosAsync(cancellationToken);
        var paises = await _paises.ObtenerTodosAsync(cancellationToken);
        var estados = await _estados.ObtenerTodosAsync(cancellationToken);
        var nombresPais = paises.ToDictionary(p => p.Id, p => p.Nombre);
        var nombresEstado = estados.ToDictionary(e => e.Id, e => e.Nombre);

        return entidades
            .Select(entidad => new CiudadDto
            {
                Id = entidad.Id,
                Codigo = entidad.Codigo,
                Nombre = entidad.Nombre,
                Descripcion = entidad.Descripcion ?? string.Empty,
                PaisId = entidad.PaisId,
                PaisNombre = nombresPais.TryGetValue(entidad.PaisId, out var np) ? np : string.Empty,
                EstadoId = entidad.EstadoId,
                EstadoNombre = nombresEstado.TryGetValue(entidad.EstadoId, out var ne) ? ne : string.Empty,
                Activo = entidad.Activo
            })
            .ToList();
    }
}
