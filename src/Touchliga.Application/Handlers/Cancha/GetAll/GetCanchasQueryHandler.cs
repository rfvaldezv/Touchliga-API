using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Cancha.GetAll;

namespace Touchliga.Application.Handlers.Cancha.GetAll;

public sealed class GetCanchasQueryHandler : IRequestHandler<GetCanchasQuery, IReadOnlyList<CanchaDto>>
{
    private readonly ICanchaRepository _repository;

    public GetCanchasQueryHandler(
        ICanchaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<CanchaDto>> Handle(
        GetCanchasQuery request,
        CancellationToken cancellationToken)
    {
        var entidades = await _repository.ObtenerTodosAsync(cancellationToken);

        return entidades
            .Select(entidad => new CanchaDto
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
