using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Pais.GetAll;

namespace Touchliga.Application.Handlers.Pais.GetAll;

public sealed class GetPaisesQueryHandler : IRequestHandler<GetPaisesQuery, IReadOnlyList<PaisDto>>
{
    private readonly IPaisRepository _repository;

    public GetPaisesQueryHandler(
        IPaisRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PaisDto>> Handle(
        GetPaisesQuery request,
        CancellationToken cancellationToken)
    {
        var entidades = await _repository.ObtenerTodosAsync(cancellationToken);

        return entidades
            .Select(entidad => new PaisDto
            {
                Id = entidad.Id,
                Codigo = entidad.Codigo,
                Nombre = entidad.Nombre,
                Descripcion = entidad.Descripcion ?? string.Empty,
                Activo = entidad.Activo
            })
            .ToList();
    }
}
