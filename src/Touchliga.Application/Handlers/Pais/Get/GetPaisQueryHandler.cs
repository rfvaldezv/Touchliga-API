using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Pais.Get;

namespace Touchliga.Application.Handlers.Pais.Get;

public sealed class GetPaisQueryHandler : IRequestHandler<GetPaisQuery, PaisDto>
{
    private readonly IPaisRepository _repository;

    public GetPaisQueryHandler(
        IPaisRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaisDto> Handle(
        GetPaisQuery request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Pais");

        return new PaisDto
        {
            Id = entidad.Id,
            Codigo = entidad.Codigo,
            Nombre = entidad.Nombre,
            Descripcion = entidad.Descripcion ?? string.Empty,
            Activo = entidad.Activo
        };
    }
}
