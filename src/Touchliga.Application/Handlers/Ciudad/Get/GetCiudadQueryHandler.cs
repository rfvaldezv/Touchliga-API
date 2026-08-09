using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Ciudad.Get;

namespace Touchliga.Application.Handlers.Ciudad.Get;

public sealed class GetCiudadQueryHandler : IRequestHandler<GetCiudadQuery, CiudadDto>
{
    private readonly ICiudadRepository _repository;
    private readonly IPaisRepository _paises;
    private readonly IEstadoRepository _estados;

    public GetCiudadQueryHandler(
        ICiudadRepository repository,
        IPaisRepository paises,
        IEstadoRepository estados)
    {
        _repository = repository;
        _paises = paises;
        _estados = estados;
    }

    public async Task<CiudadDto> Handle(
        GetCiudadQuery request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Ciudad");

        var pais = await _paises.ObtenerPorIdAsync(entidad.PaisId, cancellationToken);
        var estado = await _estados.ObtenerPorIdAsync(entidad.EstadoId, cancellationToken);

        return new CiudadDto
        {
            Id = entidad.Id,
            Codigo = entidad.Codigo,
            Nombre = entidad.Nombre,
            Descripcion = entidad.Descripcion ?? string.Empty,
            PaisId = entidad.PaisId,
            PaisNombre = pais?.Nombre ?? string.Empty,
            EstadoId = entidad.EstadoId,
            EstadoNombre = estado?.Nombre ?? string.Empty,
            Activo = entidad.Activo
        };
    }
}
