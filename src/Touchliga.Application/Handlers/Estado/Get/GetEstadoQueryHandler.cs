using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Estado.Get;

namespace Touchliga.Application.Handlers.Estado.Get;

public sealed class GetEstadoQueryHandler : IRequestHandler<GetEstadoQuery, EstadoDto>
{
    private readonly IEstadoRepository _repository;
    private readonly IPaisRepository _paises;

    public GetEstadoQueryHandler(
        IEstadoRepository repository,
        IPaisRepository paises)
    {
        _repository = repository;
        _paises = paises;
    }

    public async Task<EstadoDto> Handle(
        GetEstadoQuery request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Estado");

        var pais = await _paises.ObtenerPorIdAsync(entidad.PaisId, cancellationToken);

        return new EstadoDto
        {
            Id = entidad.Id,
            Codigo = entidad.Codigo,
            Nombre = entidad.Nombre,
            Descripcion = entidad.Descripcion ?? string.Empty,
            PaisId = entidad.PaisId,
            PaisNombre = pais?.Nombre ?? string.Empty,
            Activo = entidad.Activo
        };
    }
}
